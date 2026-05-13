
using FluentAssertions;
using Microsoft.Extensions.Logging;
using SideQuest.BLL.Enums;
using SideQuest.BLL.Models;
using SideQuest.BLL.Services;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SideQuest_Test.SideQuestBLL.Tests.Models
{
    public class MapTest
    {

        private readonly MapService _mapService = new MapService();

        private bool ValidateCoordinates(double lat, double lon)
        {
            if (lat < -90 || lat > 90) return false;
            if (lon < -180 || lon > 180) return false;
            if (lat == 0 && lon == 0) return false;

            return true;
        }

        private bool ValidateIsClujArea(double lat, double lon)
        {
            bool isLatOk = lat >= 46.70 && lat <= 46.85;
            bool isLonOk = lon >= 23.40 && lon <= 23.75;

            return isLatOk && isLonOk;
        }


        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P1")]
        public void FilterEvents_WhenZoneIsManastur_ShouldReturnOnlyEventsFromThatSpecificDistrict()
        {
            var event1 = new Event { Title = "Baschet Baza Unirea", EventZone = Zone.Manastur };
            var event2 = new Event { Title = "Ieșire la cafea", EventZone = Zone.Centru };
            var event3 = new Event { Title = "Ping-pong Mănăștur", EventZone = Zone.Manastur };

            var allEvents = new List<Event> { event1, event2, event3 };
            var targetZone = Zone.Manastur;

            var result = allEvents.Where(e => e.EventZone == targetZone).ToList();

            result.Should().HaveCount(2, "because there are only two events scheduled in Mănăștur");

            result.Should().AllSatisfy(e => e.EventZone
                .Should().Be(targetZone, "because the filter must exclude events from other districts"));

            result.Should().NotContain(event2, "because Centru is not Mănăștur");
        }


        [Theory]
        [InlineData(Zone.Manastur, Category.Sports, 1)]
        [InlineData(Zone.Manastur, Category.Culture, 0)]
        [InlineData(Zone.Centru, Category.Culture, 1)]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P1")]
        public void FilterEvents_ByZoneAndCategory_ShouldReturnMatchingEvents(Zone searchZone, Category searchCategory, int expectedCount)
        {
            var event1 = new Event { Title = "Fotbal Baza Unirea", EventZone = Zone.Manastur, Category = Category.Sports };
            var event2 = new Event { Title = "Muzeul de Artă", EventZone = Zone.Centru, Category = Category.Culture };
            var event3 = new Event { Title = "Curs de Câini", EventZone = Zone.Manastur, Category = Category.Other };

            var allEvents = new List<Event> { event1, event2, event3 };

            var result = allEvents
                .Where(e => e.EventZone == searchZone && e.Category == searchCategory)
                .ToList();

            result.Should()
                .HaveCount(expectedCount,
                $"because filtering by {searchZone} and {searchCategory} should yield exactly {expectedCount} results");

            if (expectedCount > 0)
            {
                result.Should().AllSatisfy(e =>
                {
                    e.EventZone.Should().Be(searchZone);
                    e.Category.Should().Be(searchCategory);
                }, "because every result must satisfy both filter conditions simultaneously");
            }
        }


        [Theory]
        [InlineData(Zone.Manastur)]
        [InlineData(Zone.Centru)]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P2")]
        public void FilterEvents_WhenNoMatchExists_ShouldReturnEmptyList(Zone searchZone)
        {
            List<Event> allEvents;

            if (searchZone == Zone.Manastur)
            {
                allEvents = new List<Event>
                {
                    new Event { Title = "Party Centru", EventZone = Zone.Centru },
                    new Event { Title = "Tenis Gheorgheni", EventZone = Zone.Gheorgheni }
                };
            }

            else
            {
                allEvents = new List<Event>();
            }

            var result = allEvents.Where(e => e.EventZone == searchZone).ToList();

            result.Should().NotBeNull("because the filter should return an initialized empty list, not a null reference");
            result.Should().BeEmpty($"because there are no events matching the zone {searchZone} in the current context");
            result.Should().HaveCount(0, "because a mismatching filter must exclude all items");
        }


        [Theory]
        [InlineData(90.0, 180.0, true)]
        [InlineData(-90.0, -180.0, true)]
        [InlineData(91.0, 23.5, false)]
        [InlineData(-91.0, 23.5, false)]
        [InlineData(46.7, 181.0, false)]
        [InlineData(0.0, 0.0, false)]
        [Trait("Feature", "MapIntegritiy")]
        [Trait("Type", "BoundaryTest")]
        [Trait("Priority", "P1")]
        public void ValidateCoordinates_ShouldIdentifyPhysicalAndBusinessBounds(double lat, double lon, bool expected)
        {
            bool isValid = ValidateCoordinates(lat, lon);

            isValid.Should().Be(expected,
                $"because the coordinates ({lat}, {lon}) are {(expected ? "within" : "outside")} valid bounds");
        }


        [Theory]
        [InlineData(44.4, 26.1)]
        [InlineData(48.8, 2.3)]
        [Trait("Feature", "MapGeofencing")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P1")]

        public void CreateEvent_OutsideCluj_ShouldBeInvalid(double lat, double lon)
        {
            bool isValid = ValidateIsClujArea(lat, lon);

            isValid.Should().BeFalse($"because {lat} and {lon} should be outside the Cluj-Napoca bounding box");
        }


        [Theory]
        [InlineData(46.76, 23.59)]
        [Trait("Feature", "MapGeofencing")]
        [Trait("Type", "LogicTest")]
        [Trait("Priority", "P1")]
        public void CreateEvent_OutsideCluj_ShouldBeValid(double lat, double lon)
        {
            bool isValid = ValidateIsClujArea(lat, lon);

            isValid.Should().BeTrue($"because {lat} and {lon} should be outside the Cluj-Napoca bounding box");
        }


        [Theory]
        [InlineData(46.771, 23.589, Zone.Centru)]
        [InlineData(46.755, 23.560, Zone.Manastur)]
        [InlineData(46.778, 23.620, Zone.Marasti)]
        [InlineData(46.762, 23.608, Zone.Gheorgheni)]
        [InlineData(46.742, 23.585, Zone.Zorilor)]
        [InlineData(46.765, 23.540, Zone.Grigorescu)]
        [InlineData(46.790, 23.590, Zone.Iris)]
        [InlineData(46.750, 23.640, Zone.Sopor)]
        [Trait("Feature", "MapLogic")]
        [Trait("Type", "ConversionTest")]
        [Trait("Priority", "P1")]

        public void GetZoneFromCoordinates_ShouldReturnCorrectDistrict(double lat, double lon, Zone expectedZone)
        {
            Zone result = MapService.IdentifyZone(lat, lon);

            result.Should().Be(expectedZone, $"because the coordinates {lat}, {lon} belong to {expectedZone}");
        }


        [Theory]
        [InlineData(46.7712, 23.5892, true)]
        [InlineData(46.7550, 23.5600, true)]
        [InlineData(46.5700, 23.7800, false)]
        [InlineData(47.1400, 23.8700, false)]
        [Trait("Feature", "MapLogic")]
        [Trait("Type", "ProximityValidation")]
        public void ValidateEventProximity_ShouldReturnExpectedResult(double lat, double lon, bool expected)
        {
            bool result = MapService.IsWithinProximity(lat, lon);

            result.Should().Be(expected, $"because location ({lat}, {lon}) is {(expected ? "within" : "outside")} the 15km service radius");
        }


        [Fact]
        [Trait("Feature", "MapLogic")]
        [Trait("Type", "SortingTest")]
        [Trait("Priority", "P1")]
        public void SortEventsByProximity_ShouldReturnClosestEventsFirst()
        {
            double userLat = 46.7585;
            double userLon = 23.5592;

            var eventFar = new Event { Title = "Marasti Party", Lat = 46.7760, Lon = 23.6210 };
            var eventClose = new Event { Title = "Manastur Gym", Lat = 46.7590, Lon = 23.5610 };
            var eventMedium = new Event { Title = "City Center Concert", Lat = 46.7710, Lon = 23.5890 };

            var eventList = new List<Event> { eventFar, eventMedium, eventClose };

            var sortedList = MapService.SortEventsByProximity(userLat, userLon, eventList);

            sortedList[0].Title.Should().Be("Manastur Gym");
            sortedList[1].Title.Should().Be("City Center Concert");
            sortedList[2].Title.Should().Be("Marasti Party");
        }


        [Fact]
        [Trait("Feature", "MapLogic")]
        [Trait("Type", "RadiusFiltering")]
        [Trait("Priority", "P1")]
        public void FilterEventsByRadius_ShouldIncludeEventsAcrossDistrictBoundaries()
        {
            double userLat = 46.7610;
            double userLon = 23.5710;
            double searchRadius = 1.0;

            var eventManastur = new Event { Title = "Manastur Coffee Shop", Lat = 46.7600, Lon = 23.5690 };

            var eventCenter = new Event { Title = "Center Art Gallery", Lat = 46.7630, Lon = 23.5730 };

            var eventFar = new Event { Title = "Marasti Concert", Lat = 46.7780, Lon = 23.6210 };

            var allEvents = new List<Event> { eventManastur, eventCenter, eventFar };

            var results = MapService.FilterEventsByRadius(userLat, userLon, allEvents, searchRadius);

            results.Should().HaveCount(2, "because only two events are within the 1km radius, regardless of their district");
            results.Should().Contain(e => e.Title == "Manastur Coffee Shop");
            results.Should().Contain(e => e.Title == "Center Art Gallery");
            results.Should().NotContain(e => e.Title == "Marasti Concert");
        }


        [Fact]
        [Trait("Feature", "MapLogic")]
        [Trait("Type", "BoundaryPriority")]
        [Trait("Priority", "P2")]
        public void IdentifyZone_WhenOnExactBoundary_ShouldReturnFirstMatchingZone()
        {

            double boundaryLat = 46.76;
            double lon = 23.58;

            Zone result = MapService.IdentifyZone(boundaryLat, lon);

            result.Should().Be(Zone.Centru, "because it is the first defined zone that covers the 46.76 boundary");
            result.Should().NotBe(Zone.Unknown, "because the system must always assign a zone to boundary coordinates");
        }


        [Fact]
        [Trait("Feature", "MapLogic")]
        [Trait("Type", "PrecisionTest")]
        [Trait("Priority", "P3")]
        public void IdentifyZone_WithExcessiveDecimals_ShouldStillMapCorrecty()
        {
            double extremeLat = 46.771234567890123456789;
            double extremeLon = 23.589234567890123456789;

            Zone result = MapService.IdentifyZone(extremeLat, extremeLon);

            result.Should().Be(Zone.Centru, "because the system should handle high-precision floats by rounding them naturally");
        }


        [Theory]
        [InlineData("46.77", "abc")]
        [InlineData("SELECT * FROM Users", "23")]
        [InlineData("46,77", "23.58")]
        [InlineData("", " ")]
        [Trait("Feature", "Security")]
        [Trait("Type", "SanitizationTest")]
        [Trait("Priority", "P1")]
        public void TryParseCoordinates_WithNonNumericInput_ShouldReturnFalse(string rawLat, string rawLon)
        {
            bool result = MapService.TryParseCoordinates(rawLat, rawLon, out double lat, out double lon);

            result.Should().BeFalse($"because the input '{rawLat}' or '{rawLon}' is not a valid numeric coordinate");
        }



        [Fact]
        [Trait("Feature", "MapUX")]
        [Trait("Type", "CollisionTest")]
        [Trait("Priority", "P2")]
        public void IdentifyDuplicateCoordinates_ShouldDetectOverlappingEvents()
        {
            var eventA = new Event { Title = "Untold Festival", Lat = 46.7682, Lon = 23.5723 };
            var eventB = new Event { Title = "Local Football Match", Lat = 46.7682, Lon = 23.5723 };

            bool isOverlapping = MapService.AreCoordinatesIdentical(eventA, eventB);

            isOverlapping.Should().BeTrue("because both events share the exact same GPS coordinates and will overlap on the map");
        }


        // --- CATEGORY FILTERING ---


        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "Functional")]
        [Trait("Priority", "P1")]
        public void Filter_ClickingSocial_ShouldOnlyShowSocialMarkersOnMap()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .ApplyFilter(Category.Social)
                .VisibleMarkers.Should().AllSatisfy(m => m.Category.Should().Be(Category.Social),
                    "because the Social filter must exclude all other event types");

        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "Functional")]
        [Trait("Priority", "P1")]
        public void Filter_ClickingAll_ShouldResetAndShowAllCategories()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .ApplyFilter(Category.Social)
                .ApplyFilter(Category.Other)
                .VisibleMarkers.Should().HaveCount(SeedData.GetClujEvents().Count,
                    "because clearing or changing filters should eventually allow seeing all events");

        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Filter_MultipleSelection_ShouldShowUnionOfSelectedCategories()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .ApplyFilters(new[] { Category.Sports, Category.Gaming })
                .VisibleMarkers.Should().OnlyContain(m => m.Category == Category.Sports || m.Category == Category.Gaming);

        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "Boundary")]
        [Trait("Priority", "P2")]
        public void Filter_EmptyCategory_ShouldShowNoMarkersButKeepMapFunctional()
            => _mapService
                .WithEvents(new List<Event>())
                .ApplyFilter(Category.Educational)
                .Execute(s => s.VisibleMarkers.Should().BeEmpty())
                .IsMapResponsive.Should().BeTrue("because an empty result set should not freeze the UI");

        [Theory]
        [InlineData(320)]
        [InlineData(414)]
        [Trait("Feature", "MapUX")]
        [Trait("Type", "Layout")]
        public void Filter_ScrollCategories_ShouldWorkSmoothlyOnSmallScreens(int screenWidth)
            => _mapService
                .SetViewportWidth(screenWidth)
                .GetCategoryBarState()
                .IsScrollable.Should().BeTrue("because the category list must be scrollable on mobile screens");


        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "UX_Toggle")]
        public void Filter_ToggleOff_ShouldShowAllEventsAgain()
           => _mapService
               .WithEvents(SeedData.GetClujEvents())
               .ApplyFilter(Category.Social)
               .ApplyFilter(Category.Social)
               .VisibleMarkers.Should().HaveCount(SeedData.GetClujEvents().Count,
                   "because clicking an active filter twice should deselect it and show everything");

        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "Functional")]
        public void Filter_SwitchingDirectly_ShouldReplacePreviousFilter()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .ApplyFilter(Category.Sports)
                .ApplyFilter(Category.Gaming) 
                .VisibleMarkers.Should().AllSatisfy(m => m.Category.Should().Be(Category.Gaming),
                    "because the UI should replace the previous category with the new one, not stack them");

        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "UI_Validation")]
        public void Filter_NoEventsInCategory_ShouldShowEmptyMapMessage()
        {
            var result = _mapService
                .WithEvents(new List<Event>())
                .ApplyFilter(Category.Educational);

            result.VisibleMarkers.Should().BeEmpty();
            result.EmptyStateMessage.Should().Be("Nu am găsit evenimente",
                "because the user needs feedback when a filter returns no results");
        }

        [Fact]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "Logic_Intersection")]
        public void Filter_SearchByText_AndCategory_ShouldIntersect()
        {
            var events = new List<Event> {
                new Event { Title = "Meci Fotbal", Category = Category.Sports },
                new Event { Title = "Antrenament Baschet", Category = Category.Sports },
                new Event { Title = "Meci Gaming", Category = Category.Gaming }
            };

            var result = _mapService
                .WithEvents(events)
                .SearchText("Meci")
                .ApplyFilter(Category.Sports);

            result.VisibleMarkers.Should().HaveCount(1);
            result.VisibleMarkers.First().Label.Should().Be("Meci Fotbal",
                "because the results should match both the search text and the category simultaneously");
        }

        [Theory]
        [InlineData(Category.Gaming, "🎲")]
        [InlineData(Category.Sports, "⚽")]
        [Trait("Feature", "MapFiltering")]
        [Trait("Type", "UI_Consistency")]
        public void Filter_CategoryIcons_ShouldMatchDesignLabels(Category category, string expectedEmoji)
        {
            var @event = new Event { Title = "Test", Category = category, Emoji = expectedEmoji };
            var marker = _mapService.CreateMarker(@event);

            marker.Emoji.Should().Be(expectedEmoji,
                $"because the icon for {category} must be consistent with the design system");
        }


        // --- MARKER SYSTEM ---


        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "UIValidation")]
        [Trait("Priority", "P1")]
        public void Marker_ShouldDisplayCategoryIconAndTitleCorrectly()
        {
            var @event = new Event
            {
                Title = "Board Games Night",
                Category = Category.Gaming,
                Emoji = "🎲"
            };

            var marker = _mapService.CreateMarker(@event);

            marker.Icon.Should().Be("gaming_icon");
            marker.Label.Should().Be("Board Games Night");
        }

        [Theory]
        [InlineData(Category.Sports, "Green")]
        [InlineData(Category.Social, "Red")]
        [InlineData(Category.Outdoor, "Blue")]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "UIValidation")]
        public void Marker_Color_ShouldMatchCategoryTheme(Category category, string expectedColor)
            => _mapService.CreateMarker(new Event { Category = category, Title = "Test" })
                .ThemeColor.Should().Be(expectedColor, $"because {category} color theme must match the UI design");

        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "Interaction")]
        [Trait("Priority", "P1")]
        public void Marker_Click_ShouldOpenEventDetails()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .ClickMarker("Meci de Fotbal")
                .NavigationPath.Should().Contain("details/Meci de Fotbal");

        [Fact]
        [Trait("Feature", "MapLogic")]
        [Trait("Type", "Clustering")]
        [Trait("Priority", "P2")]
        public void Marker_Overlap_ShouldBeHandledByClustering()
        {
            var overlappingEvents = new List<Event> {
                new Event { Lat = 46.77, Lon = 23.58, Title = "Event 1", Category = Category.Friends },
                new Event { Lat = 46.77, Lon = 23.58, Title = "Event 2", Category = Category.Social }
            };

            _mapService.WithEvents(overlappingEvents)
                .GetVisibleMarkers().Should().HaveCount(1, "because overlapping events at same coords should be clustered");
        }

        [Fact]
        [Trait("Feature", "MapPerformance")]
        [Trait("Type", "Optimization")]
        [Trait("Priority", "P2")]
        public void Marker_OffScreen_ShouldNotRenderToSavePerformance()
            => _mapService
                .WithEvents(new List<Event> { new Event { Lat = 44.0, Lon = 20.0, Title = "Departe", Category = Category.Other } })
                .FocusOnCluj()
                .RenderedMarkersCount.Should().Be(0, "because markers outside the viewport should not consume resources");


        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "Interaction")]
        [Trait("Priority", "P2")]
        public void Marker_TapOnCluster_ShouldZoomIn()
        {
            var initialZoom = _mapService.CurrentZoom;

            _mapService
                .ClickCluster(5) 
                .CurrentZoom.Should().BeGreaterThan(initialZoom,
                    "because tapping a cluster should zoom in to reveal individual markers");
        }

        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "DataSync")]
        [Trait("Priority", "P2")]
        public void Marker_UpdateEventData_ShouldRefreshMarkerLabel()
        {
            var @event = new Event { Title = "Meci Fotbal", Category = Category.Sports };
            _mapService.WithEvents(new List<Event> { @event });

            @event.Title = "Finală Fotbal";
            _mapService.SyncVisibleMarkers();

            _mapService.VisibleMarkers.First().Label.Should().Be("Finală Fotbal",
                "because the map should reflect real-time changes in event data");
        }

        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "UX_ZIndex")]
        [Trait("Priority", "P2")]
        public void Marker_ZIndex_SelectedMarkerShouldBeOnTop()
        {
            var result = _mapService
                .WithEvents(SeedData.GetClujEvents())
                .ClickMarker("Meci de Fotbal");

            result.GetMarkerByTitle("Meci de Fotbal").ZIndex.Should().Be(999,
                "because the selected marker must appear above all others to remain visible");
        }

        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "Logic_Proximity")]
        [Trait("Priority", "P2")]
        public void Marker_DistanceLabel_ShouldBeCalculatedFromUserLocation()
        {
            double userLat = 46.7712; //Centru
            double userLon = 23.5892;

            var @event = new Event { Lat = 46.7712, Lon = 23.5992, Title = "Aproape" };

            var marker = _mapService
                .SetUserLocation(userLat, userLon)
                .CreateMarker(@event);

            marker.DistanceLabel.Should().NotBeNullOrEmpty();
            marker.DistanceLabel.Should().Contain("km", "because users need to see the distance to the event");
        }


        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "Boundary")]
        [Trait("Priority", "P2")]
        public void Marker_InvalidCoordinates_ShouldNotBeAddedToMap()
        {
            var @event = new Event { Lat = 91.0, Lon = 181.0, Title = "Invalid Location" };

            _mapService
                .WithEvents(new List<Event> { @event })
                .VisibleMarkers.Should().BeEmpty("because coordinates outside global bounds are invalid");
        }

        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "UIValidation")]
        [Trait("Priority", "P3")]
        public void Marker_Emoji_ShouldMatchEventDefinition()
        {
            var @event = new Event { Title = "Party", Emoji = "🥳", Category = Category.Social };

            var marker = _mapService.CreateMarker(@event);

            marker.Emoji.Should().Be("🥳", "because the marker must visually represent the event's specific emoji");
        }

        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "Functional")]
        [Trait("Priority", "P1")]
        public void Marker_ClearEvents_ShouldRemoveAllMarkersFromMap()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .WithEvents(new List<Event>())
                .VisibleMarkers.Should().BeEmpty("because re-initializing with an empty list must clear the UI");

        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "Performance")]
        [Trait("Priority", "P2")]
        public void Marker_MassiveLoad_ShouldHandleLargeNumberOfEvents()
        {
            var manyEvents = Enumerable.Range(0, 1000)
                .Select(i => new Event { Title = $"Event {i}", Lat = 46.77, Lon = 23.58 })
                .ToList();

            _mapService
                .WithEvents(manyEvents)
                .VisibleMarkers.Should().HaveCount(1000, "because the service must support high density event data");
        }

        [Fact]
        [Trait("Feature", "MapMarkers")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Marker_InitialState_ShouldHaveDefaultZIndex()
        {
            var @event = new Event { Title = "Normal Event", Category = Category.Other };

            var marker = _mapService.CreateMarker(@event);

            marker.ZIndex.Should().Be(1, "because markers should start at the lowest layer before interaction");
        }


        // --- NEARBY PANEL ---


        [Fact]
        [Trait("Feature", "NearbyPanel")]
        [Trait("Type", "Functional")]
        [Trait("Priority", "P1")]
        public void NearbyPanel_ShouldSortEventsByPhysicalDistance()
        {
            var farEvent = new Event { Title = "Far", Lat = 47.0, Lon = 24.0 };
            var closeEvent = new Event { Title = "Close", Lat = 46.77, Lon = 23.59 };

            var result = _mapService
                .SetUserLocation(46.7712, 23.5892)
                .WithEvents(new List<Event> { farEvent, closeEvent })
                .GetNearbyPanelEvents();

            result.First().Title.Should().Be("Close", "because the closest events must appear first in the panel");
        }

        [Fact]
        [Trait("Feature", "NearbyPanel")]
        [Trait("Type", "DynamicUpdate")]
        [Trait("Priority", "P2")]
        public void NearbyPanel_DistanceDisplay_ShouldUpdateAsUserMoves()
        {
            var @event = new Event { Lat = 46.7712, Lon = 23.6000, Title = "Target" };

            var initialDistance = _mapService
                .SetUserLocation(46.7712, 23.5892)
                .CreateMarker(@event).DistanceLabel;

            var updatedDistance = _mapService
                .SetUserLocation(46.7712, 23.5990)
                .CreateMarker(@event).DistanceLabel;

            updatedDistance.Should().NotBe(initialDistance, "because labels must reflect the new real-time distance");
        }

        [Fact]
        [Trait("Feature", "NearbyPanel")]
        [Trait("Type", "UI_State")]
        [Trait("Priority", "P2")]
        public void NearbyPanel_Expand_ShouldRevealMoreEvents()
            => _mapService
                .SetPanelState(PanelState.Collapsed)
                .Execute(s => s.VisiblePanelItems.Should().BeLessThanOrEqualTo(3))
                .SetPanelState(PanelState.Expanded)
                .VisiblePanelItems.Should().BeGreaterThan(3, "because expanding the sheet should load the full list");

        [Fact]
        [Trait("Feature", "NearbyPanel")]
        [Trait("Type", "UIValidation")]
        [Trait("Priority", "P2")]
        public void NearbyPanel_EmptyState_ShouldShowMessageWhenNoEventsNearby()
            => _mapService
                .SetUserLocation(0, 0) //to simulate a location far from any events
                .WithEvents(SeedData.GetClujEvents())
                .EmptyStateMessage.Should().Be("Nu am găsit evenimente în apropiere");

        [Fact]
        [Trait("Feature", "ProximitySystem")]
        [Trait("Type", "Math")]
        [Trait("Priority", "P1")]
        public void Distance_Calculation_ShouldBeAccurateWithin10Meters()
        {
            var userLat = 46.7712;
            var userLon = 23.5892;
            var eventLat = 46.7713;
            var eventLon = 23.5893;

            var distance = _mapService.CalculateRawDistance(userLat, userLon, eventLat, eventLon);

            distance.Should().BeInRange(10, 20, "because the Haversine formula must remain precise at small scales");
        }

        [Theory]
        [InlineData(0.5, "500 m")]
        [InlineData(2.5, "2.5 km")]
        [Trait("Feature", "ProximitySystem")]
        [Trait("Type", "Logic")]
        public void Distance_UnitConversion_ShouldShowKmUnder10kmAndMetersUnder1km(double km, string expectedLabel)
        {
            var label = _mapService.FormatDistanceLabel(km);
            label.Should().Be(expectedLabel, "because the UI must switch units for better readability");
        }

        [Fact]
        [Trait("Feature", "ProximitySystem")]
        [Trait("Type", "Safety")]
        [Trait("Priority", "P1")]
        public void GPS_LossOfSignal_ShouldShowWarningToUser()
            => _mapService
                .SetGpsStatus(GpsStatus.Lost)
                .SystemWarning.Should().Be("Semnal GPS pierdut", "because users must be notified when location data is stale");

        [Fact]
        [Trait("Feature", "ProximitySystem")]
        [Trait("Type", "Performance")]
        [Trait("Priority", "P3")]
        public void GPS_BackgroundUpdate_ShouldNotDrainBatteryExcessively()
            => _mapService
                .SetUpdateInterval(TimeSpan.FromSeconds(30))
                .BatteryImpactScore.Should().BeLessThan(5, "because high-frequency updates must be throttled in background");

        [Fact]
        [Trait("Feature", "ProximitySystem")]
        [Trait("Type", "Boundary")]
        [Trait("Priority", "P2")]
        public void Location_OutsideCluj_ShouldAskToChangeCity()
        {
            var bucharestLat = 44.4268;
            var bucharestLon = 26.1025;

            _mapService
                .SetUserLocation(bucharestLat, bucharestLon)
                .CityPromptVisible.Should().BeTrue("because the app should suggest switching cities when the user is far away");
        }


        // --- IN-MEMORY / DB ---


        [Fact]
        [Trait("Feature", "DataIntegrity")]
        [Trait("Type", "Lifecycle")]
        [Trait("Priority", "P1")]
        public void Event_ExpiredTime_ShouldAutomaticallyDisappearFromMap()
            => _mapService
                .WithEvents(SeedData.GetExpiredEvents())
                .ApplyLifeCycleFilters()
                .VisibleMarkers.Count.Should().Be(0);

        [Fact]
        [Trait("Feature", "DataIntegrity")]
        [Trait("Type", "RealTime")]
        [Trait("Priority", "P1")]
        public void Event_Update_ShouldReflectInstantlyOnMapWithoutReload()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .UpdateEventTitle(eventId: 1, newTitle: "Updated Title")
                .VisibleMarkers.First(m => m.Id == 1).Label.Should().Be("Updated Title");

        [Fact]
        [Trait("Feature", "DataIntegrity")]
        [Trait("Type", "EdgeCase")]
        [Trait("Priority", "P2")]
        public void Event_Concurrency_ShouldHandleTwoEventsAtExactSameCoordinates()
            => _mapService
                .WithEvents(new List<Event> 
                {
                    new Event { Lat = 46.77, Lon = 23.58 },
                    new Event { Lat = 46.77, Lon = 23.58 }
                })
                .VisibleMarkers.Count
                .Should().Be(2);

        [Fact]
        [Trait("Feature", "DataIntegrity")]
        [Trait("Type", "UI_Action")]
        [Trait("Priority", "P2")]
        public void Event_Creation_ButtonPlus_ShouldOpenCorrectForm()
            => _mapService
                .ClickMarker("new") 
                .NavigationPath.Should().Be("details/new");

        [Fact]
        [Trait("Feature", "DataIntegrity")]
        [Trait("Type", "UI_Sync")]
        [Trait("Priority", "P2")]
        public void Event_CountBadge_ShouldMatchNumberOfMarkersVisible()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .ApplyFilter(Category.Sports)
                .RenderedMarkersCount 
                .Should().Be(_mapService.VisibleMarkers.Count);

        // --- ADVANCED LOGIC TESTS ---


        [Fact]
        [Trait("Feature", "Filters")]
        [Trait("Type", "Persistence")]
        [Trait("Priority", "P2")]
        public void Filter_PersistentState_ShouldRemainSelectedAfterOpeningEvent()
            => _mapService
                .ApplyFilter(Category.Culture)
                .ClickMarker("1") 
                .VisibleMarkers.All(m => m.Category == Category.Culture)
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Search")]
        [Trait("Type", "Filtering")]
        [Trait("Priority", "P2")]
        public void Search_ByTitle_ShouldFilterMapMarkers()
            => _mapService
                .WithEvents(SeedData.GetClujEvents())
                .SearchText("Concert") 
                .VisibleMarkers.All(m => m.Label.Contains("Concert", StringComparison.OrdinalIgnoreCase))
                .Should().BeTrue();

        [Fact]
        [Trait("Feature", "Search")]
        [Trait("Type", "Navigation")]
        [Trait("Priority", "P2")]
        public void Search_ByLocationName_ShouldCenterMapOnResult()
            => _mapService
                .SearchText("Centru") 
                .VisibleMarkers.Count
                .Should().BeGreaterThan(0);

        [Fact]
        [Trait("Feature", "Filters")]
        [Trait("Type", "Logic")]
        [Trait("Priority", "P2")]
        public void Filter_Combination_CategoryAndTime_ShouldWork()
            => _mapService
                .ApplyFilter(Category.Social)
                .ApplyLifeCycleFilters() 
                .VisibleMarkers.Count
                .Should().BeLessThanOrEqualTo(SeedData.GetClujEvents().Count);

        [Fact]
        [Trait("Feature", "Filters")]
        [Trait("Type", "RealTime")]
        [Trait("Priority", "P2")]
        public void Filter_DistanceSlider_ShouldUpdateMapInRealTime()
            => _mapService
                .SetUserLocation(46.7712, 23.5892)
                .FilterByDistance(1) 
                .VisibleMarkers.All(m => m.Distance <= 1).Should().BeTrue();


    }





    public static class SeedData
    {
        public static List<Event> GetClujEvents()
        {
            return new List<Event>
            {
                new Event { Title = "Meci de Fotbal", Category = Category.Sports, Lat = 46.779, Lon = 23.577, EventZone = Zone.Grigorescu, Emoji = "⚽" },
                new Event { Title = "Board Games Night", Category = Category.Gaming, Lat = 46.755, Lon = 23.560, EventZone = Zone.Manastur, Emoji = "🎲" },
                new Event { Title = "Ieșire în Centru", Category = Category.Social, Lat = 46.771, Lon = 23.589, EventZone = Zone.Centru, Emoji = "🍻" }
            };
        }
    }
}
       