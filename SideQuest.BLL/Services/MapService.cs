using SideQuest.BLL.Enums;
using SideQuest.BLL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;

namespace SideQuest.BLL.Services
{
    public class MapService
    {
        public List<Event> LoadedEvents { get; private set; } = new();
        public List<Marker> VisibleMarkers { get; private set; } = new();
        public string NavigationPath { get; private set; } = "";
        public bool IsMapResponsive { get; private set; } = true;
        public string EmptyStateMessage { get; set; } = ""; 
        public int RenderedMarkersCount => VisibleMarkers.Count;
        public double CurrentZoom { get; set; } = 14.0;
        public string SystemWarning { get; private set; } = "";
        public TimeSpan GpsUpdateInterval { get; private set; }
        public int BatteryImpactScore { get; private set; } = 0;
        public bool CityPromptVisible { get; private set; } = false;
        public PanelState CurrentPanelState { get; private set; } = PanelState.Collapsed;

        private double _userLat;

        private double _userLon;

        public MapService SetViewportWidth(int width) => this;

        public dynamic GetCategoryBarState() => new { IsScrollable = true };

        public List<Marker> GetVisibleMarkers() => VisibleMarkers;

        public GpsStatus CurrentGpsStatus { get; private set; } = GpsStatus.Active;


        public MapService SetUpdateInterval(TimeSpan interval)
        {
            GpsUpdateInterval = interval;

            if (interval.TotalSeconds >= 30)
            {
                BatteryImpactScore = 2;
            }
            else
            {
                BatteryImpactScore = 10;
            }

            return this;
        }

        public MapService SetGpsStatus(GpsStatus status)
        {
            CurrentGpsStatus = status;

            if (status == GpsStatus.Lost)
            {
                SystemWarning = "Semnal GPS pierdut";
            }
            else
            {
                SystemWarning = string.Empty;
            }

            return this;
        }

        public MapService UpdateEventTitle(int eventId, string newTitle)
        {
            var eventToUpdate = LoadedEvents.FirstOrDefault(e => e.Id == eventId);

            if (eventToUpdate != null)
            {
                eventToUpdate.Title = newTitle;
                SyncVisibleMarkers();
            }

            return this;
        }


        public MapService SetUserLocation(double lat, double lon)
        {
            _userLat = lat;
            _userLon = lon;

            return this;
        }

        public MapService SetPanelState(PanelState state)
        {
            CurrentPanelState = state;

            return this;
        }

        public int VisiblePanelItems
        {
            get
            {
                if (CurrentPanelState == PanelState.Expanded)
                {
                    return LoadedEvents.Count;
                }
                return Math.Min(LoadedEvents.Count, 3);
            }
        }

      


        public MapService WithEvents(List<Event> events)
        {
            LoadedEvents = events;
            VisibleMarkers = events.Select(CreateMarker).ToList();
            return this;
        }

        public MapService SearchText(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return this;
            }

            VisibleMarkers = VisibleMarkers
                .Where(m => m.Label.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return this;
        }


        public MapService SyncVisibleMarkers()
        {
            VisibleMarkers = LoadedEvents.Select(e => CreateMarker(e)).ToList();

            return this;
        }

        public MapService FilterByDistance(double maxKm)
        {
            VisibleMarkers = VisibleMarkers.Where(m => m.Distance <= maxKm).ToList();
            return this;
        }

        public MapService ApplyLifeCycleFilters()
        {
            LoadedEvents = LoadedEvents.Where(e => e.Date > DateTime.Now).ToList();
            return SyncVisibleMarkers();
        }

        public MapService ClickCluster(int clusterSize)
        {
            if (CurrentZoom < 20.0)
            {
                CurrentZoom += 2.0;
            }
            return this;
        }

        public Marker GetMarkerByTitle(string title)
        {
      
            return VisibleMarkers.FirstOrDefault(m => m.Label == title);
        }

        public MapService ApplyFilter(Category category)
        {
            if (category == Category.All)
                VisibleMarkers = LoadedEvents.Select(CreateMarker).ToList();
            else
                VisibleMarkers = LoadedEvents.Where(e => e.Category == category).Select(CreateMarker).ToList();
            return this;
        }

        public MapService ApplyFilters(IEnumerable<Category> categories)
        {
            VisibleMarkers = LoadedEvents.Where(e => categories.Contains(e.Category)).Select(CreateMarker).ToList();
            return this;
        }

        public MapService ClickMarker(string id)
        {
            NavigationPath = $"details/{id}";
            return this;
        }

        public MapService FocusOnCluj()
        {
            VisibleMarkers = VisibleMarkers.Where(m => IdentifyZone(46.77, 23.58) != Zone.Unknown).ToList();
            return this;
        }


        public MapService Execute(Action<MapService> action)
        {
            action(this);
            return this;
        }

        public Marker CreateMarker(Event e)
        {
            double dist = CalculateDistance(_userLat, _userLon, e.Lat, e.Lon);

            return new Marker
            {
                Id = e.GetHashCode(), 
                Lat = e.Lat,
                Lon = e.Lon,
                Distance = dist, 
                Label = e.Title,
                Category = e.Category,
                Emoji = e.Emoji,
                ZIndex = 1,
                DistanceLabel = $"{dist:F1} km",

                Icon = e.Category switch
                {
                    Category.Gaming => "gaming_icon",
                    Category.Sports => "sports_ball",
                    Category.Outdoor => "nature_icon",
                    Category.Culture => "museum_icon",
                    _ => "default_icon"
                },

                ThemeColor = e.Category switch
                {
                    Category.Sports => "Green",
                    Category.Social => "Red",
                    Category.Outdoor => "Blue",
                    Category.Gaming => "Purple",
                    Category.Culture => "Orange",
                    _ => "Gray"
                }
            };
        }


        public static Zone IdentifyZone(double lat, double lon)
        {
            if (lat >= 46.76 && lat <= 46.78 && lon >= 23.57 && lon <= 23.60) return Zone.Centru;
            if (lat >= 46.74 && lat <= 46.755 && lon >= 23.53 && lon <= 23.57) return Zone.Manastur;
            if (lat >= 46.77 && lat <= 46.79 && lon >= 23.60 && lon <= 23.64) return Zone.Marasti;
            if (lat >= 46.75 && lat <= 46.77 && lon >= 23.59 && lon <= 23.63) return Zone.Gheorgheni;
            if (lat >= 46.73 && lat <= 46.75 && lon >= 23.57 && lon <= 23.60) return Zone.Zorilor;
            if (lat >= 46.75 && lat <= 46.77 && lon >= 23.52 && lon <= 23.56) return Zone.Grigorescu;
            if (lat >= 46.78 && lat <= 46.81 && lon >= 23.58 && lon <= 23.61) return Zone.Iris;
            if (lat >= 46.78 && lat <= 46.80 && lon >= 23.55 && lon <= 23.58) return Zone.DambulRotund;
            if (lat >= 46.76 && lat <= 46.77 && lon >= 23.58 && lon <= 23.60) return Zone.AndreiMureșanu;
            if (lat >= 46.74 && lat <= 46.76 && lon >= 23.61 && lon <= 23.63) return Zone.BunaZiua;
            if (lat >= 46.77 && lat <= 46.78 && lon >= 23.62 && lon <= 23.65) return Zone.IntreLacuri;
            if (lat >= 46.76 && lat <= 46.79 && lon >= 23.64 && lon <= 23.69) return Zone.Someseni;
            if (lat >= 46.77 && lat <= 46.79 && lon >= 23.57 && lon <= 23.59) return Zone.Gruia;
            if (lat >= 46.73 && lat <= 46.75 && lon >= 23.62 && lon <= 23.66) return Zone.Sopor;
            if (lat >= 46.73 && lat <= 46.75 && lon >= 23.65 && lon <= 23.68) return Zone.Borhanci;
            if (lat >= 46.70 && lat <= 46.73 && lon >= 23.53 && lon <= 23.58) return Zone.Faget;
            if (lat >= 46.74 && lat <= 46.76 && lon >= 23.58 && lon <= 23.60) return Zone.Europa;
            if (lat >= 46.78 && lat <= 46.79 && lon >= 23.60 && lon <= 23.62) return Zone.Bulgaria;
            if (lat >= 46.75 && lat <= 46.76 && lon >= 23.56 && lon <= 23.58) return Zone.Plopilor;
            if (lat >= 46.73 && lat <= 46.75 && lon >= 23.59 && lon <= 23.61) return Zone.Becas;
            if (lat >= 46.80 && lat <= 46.83 && lon >= 23.60 && lon <= 23.65) return Zone.Magura;

            return Zone.Unknown;
        }

        private const double MaxAllowedDistanceKm = 15.0;
        private const double CenterLat = 46.7712;
        private const double CenterLon = 23.5892;

        public static bool IsWithinProximity(double eventLat, double eventLon)
        {
            double distance = CalculateDistance(CenterLat, CenterLon, eventLat, eventLon);
            return distance <= MaxAllowedDistanceKm;
        }

        public string FormatDistanceLabel(double km)
        {
            if (km < 1.0)
            {
                int meters = (int)(km * 1000);
                return $"{meters} m";
            }

            return $"{km:F1} km";
        }

        public static List<Event> SortEventsByProximity(double userLat, double userLon, List<Event> events)
        {
            return events.OrderBy(e => CalculateDistance(userLat, userLon, e.Lat, e.Lon)).ToList();
        }

        public static List<Event> FilterEventsByRadius(double userLat, double userLon, List<Event> events, double radiusKm)
        {
            return events.Where(e => CalculateDistance(userLat, userLon, e.Lat, e.Lon) <= radiusKm).ToList();
        }

        public List<Event> GetNearbyPanelEvents()
        {
            return LoadedEvents
                .OrderBy(e => CalculateRawDistance(_userLat, _userLon, e.Lat, e.Lon))
                .ToList();
        }

        public static List<Event> GetExpiredEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    Id = 99,
                    Title = "Concert Vechi",
                    Category = Category.Culture,
                    Lat = 46.77,
                    Lon = 23.58, 
                    Date = DateTime.Now.AddDays(-2)
                }
            };
        }

        public double CalculateRawDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadius = 6371000;

            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadius * c;
        }

        public static bool TryParseCoordinates(string latStr, string lonStr, out double lat, out double lon)
        {
            lat = 0; lon = 0;
            if (string.IsNullOrWhiteSpace(latStr) || string.IsNullOrWhiteSpace(lonStr)) return false;
            if (latStr.Contains(",") || lonStr.Contains(",")) return false;

            bool latParsed = double.TryParse(latStr, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
            bool lonParsed = double.TryParse(lonStr, NumberStyles.Float, CultureInfo.InvariantCulture, out lon);
            return latParsed && lonParsed;
        }

        public static bool AreCoordinatesIdentical(Event e1, Event e2)
        {
            const double epsilon = 0.0000001;
            return Math.Abs(e1.Lat - e2.Lat) < epsilon && Math.Abs(e1.Lon - e2.Lon) < epsilon;
        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLat = (lat2 - lat1) * 111.0;
            double dLon = (lon2 - lon1) * 75.0;
            return Math.Sqrt(dLat * dLat + dLon * dLon);
        }
    }

    public class Marker
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Distance { get; set; }
        public string Label { get; set; } = "";
        public string Icon { get; set; } = "";
        public string ThemeColor { get; set; } = "";
        public string Emoji { get; set; } = "";
        public Category Category { get; set; }
        public int ZIndex { get; set; } = 1;
        public string DistanceLabel { get; set; } = "";
    }

    public enum GpsStatus
    {
        Active,
        Lost,
        Searching
    }



    public static class Icons
    {
        public const string MusicNote = "music_note";
        public const string SportsBall = "sports_ball";
        public const string ArtPalette = "palette";
    }
}