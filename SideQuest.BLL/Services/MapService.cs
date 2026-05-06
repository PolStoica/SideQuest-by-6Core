using SideQuest.BLL.Enums;
using SideQuest.BLL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SideQuest.BLL.Services
{
    public static class MapService
    {
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


        public static List<Event> SortEventsByProximity(double userLat, double userLon, List<Event> events)
        {
            return events
                .OrderBy(e => CalculateDistance(userLat, userLon, e.Lat, e.Lon))
                .ToList();
        }

        public static List<Event> FilterEventsByRadius(double userLat, double userLon, List<Event> events, double radiusKm)
        {
            return events
                .Where(e => CalculateDistance(userLat, userLon, e.Lat, e.Lon) <= radiusKm)
                .ToList();
        }

 
        public static bool TryParseCoordinates(string latStr, string lonStr, out double lat, out double lon)
        {
            lat = 0;
            lon = 0;

            if (string.IsNullOrWhiteSpace(latStr) || string.IsNullOrWhiteSpace(lonStr))
                return false;

        
            if (latStr.Contains(",") || lonStr.Contains(","))
                return false;

            bool latParsed = double.TryParse(latStr, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
            bool lonParsed = double.TryParse(lonStr, NumberStyles.Float, CultureInfo.InvariantCulture, out lon);

            return latParsed && lonParsed;
        }


        public static bool AreCoordinatesIdentical(Event e1, Event e2)
        {
           
            const double epsilon = 0.0000001;

            return Math.Abs(e1.Lat - e2.Lat) < epsilon &&
                   Math.Abs(e1.Lon - e2.Lon) < epsilon;
        }


        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
          
            double dLat = (lat2 - lat1) * 111.0;
            double dLon = (lon2 - lon1) * 75.0;
            return Math.Sqrt(dLat * dLat + dLon * dLon);
        }



    }
}
