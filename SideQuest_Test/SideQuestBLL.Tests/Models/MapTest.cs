
using FluentAssertions;
using SideQuest.BLL.Enums;
using SideQuest.BLL.Models;
using SideQuest.BLL.Services;

namespace SideQuest_Test.SideQuestBLL.Tests.Models
{
    public class MapTest
    {


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

            result.Should().AllSatisfy(e =>
                e.EventZone.Should().Be(targetZone, "because the filter must exclude events from other districts"));

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
            // Act
            bool isValid = ValidateCoordinates(lat, lon);

            // Assert
            isValid.Should().Be(expected,
                $"because the coordinates ({lat}, {lon}) are {(expected ? "within" : "outside")} valid bounds");
        }

        private bool ValidateCoordinates(double lat, double lon)
        {
            if (lat < -90 || lat > 90) return false;
            if (lon < -180 || lon > 180) return false;
            if (lat == 0 && lon == 0) return false;

            return true;
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

        private bool ValidateIsClujArea(double lat, double lon)
        {
            bool isLatOk = lat >= 46.70 && lat <= 46.85;
            bool isLonOk = lon >= 23.40 && lon <= 23.75;

            return isLatOk && isLonOk;
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
    }
}

