// MapScreen.js — versiunea cu navigare la EventJoin si CreateEvent
import { StyleSheet, Text, View, TouchableOpacity, SafeAreaView, ScrollView, Platform } from 'react-native';
import { useState, useRef } from 'react';
import { StatusBar } from 'expo-status-bar';
import MapView, { Marker, PROVIDER_DEFAULT } from 'react-native-maps';

const CORAL = '#E8533A';

const CATEGORIES = [
  { id: 'all',    label: 'Toate',  emoji: null },
  { id: 'music',  label: 'Muzică', emoji: '🎵' },
  { id: 'sport',  label: 'Sport',  emoji: '⚽' },
  { id: 'art',    label: 'Artă',   emoji: '🎨' },
  { id: 'food',   label: 'Food',   emoji: '🍕' },
  { id: 'gaming', label: 'Gaming', emoji: '🎮' },
];

const EVENTS = [
  {
    id: 1,
    title: 'Concert în Parc',
    category: 'music',
    label: 'Concert',
    emoji: '🎵',
    color: '#E8533A',
    date: 'Azi 20:00',
    location: 'Parcul Central',
    distance: '0.3 km',
    spots: 18,
    total: 30,
    coordinate: { latitude: 46.7712, longitude: 23.6236 },
  },
  {
    id: 2,
    title: 'Atelier Ceramică',
    category: 'art',
    label: 'Atelier',
    emoji: '🎨',
    color: '#9B6FD4',
    date: 'Mâine 15:00',
    location: 'Centrul de Artă',
    distance: '0.7 km',
    spots: 5,
    total: 12,
    coordinate: { latitude: 46.7748, longitude: 23.6178 },
  },
  {
    id: 3,
    title: 'Fotbal Amical',
    category: 'sport',
    label: 'Fotbal',
    emoji: '⚽',
    color: '#4CAF82',
    date: 'Azi 18:30',
    location: 'Terenul din Mărăști',
    distance: '1.1 km',
    spots: 8,
    total: 22,
    coordinate: { latitude: 46.7682, longitude: 23.6295 },
  },
  {
    id: 4,
    title: 'Jazz pe Terasă',
    category: 'music',
    label: 'Concert',
    emoji: '🎷',
    color: '#F4A940',
    date: 'Sâm 21:00',
    location: 'Terasa Memorandumului',
    distance: '1.8 km',
    spots: 20,
    total: 40,
    coordinate: { latitude: 46.7735, longitude: 23.6318 },
  },
];

const CLUJ_REGION = {
  latitude: 46.7712,
  longitude: 23.6236,
  latitudeDelta: 0.03,
  longitudeDelta: 0.03,
};

function CustomMarker({ event, onPress }) {
  return (
    <Marker coordinate={event.coordinate} onPress={onPress} anchor={{ x: 0.5, y: 1 }}>
      <View style={[styles.markerBubble, { backgroundColor: event.color }]}>
        <Text style={styles.markerText}>{event.emoji} {event.label}</Text>
      </View>
      <View style={[styles.markerTail, { borderTopColor: event.color }]} />
    </Marker>
  );
}

function CategoryPill({ cat, active, onPress }) {
  return (
    <TouchableOpacity
      onPress={onPress}
      style={[styles.pill, active && styles.pillActive]}
      activeOpacity={0.75}
    >
      {cat.emoji && <Text style={styles.pillEmoji}>{cat.emoji}</Text>}
      <Text style={[styles.pillText, active && styles.pillTextActive]}>{cat.label}</Text>
    </TouchableOpacity>
  );
}

function EventCard({ event, onPress }) {
  return (
    <TouchableOpacity style={styles.eventCard} activeOpacity={0.8} onPress={onPress}>
      <View style={[styles.eventIcon, { backgroundColor: event.color + '22' }]}>
        <Text style={styles.eventIconText}>{event.emoji}</Text>
      </View>
      <View style={styles.eventInfo}>
        <Text style={styles.eventTitle} numberOfLines={1}>{event.title}</Text>
        <Text style={styles.eventMeta}>{event.date} · {event.distance} · {event.spots} locuri</Text>
      </View>
      <View style={[styles.eventBadge, { backgroundColor: event.color }]}>
        <Text style={styles.eventBadgeText}>›</Text>
      </View>
    </TouchableOpacity>
  );
}

export default function MapScreen({ navigation }) {
  const [activeCategory, setActiveCategory] = useState('all');
  const mapRef = useRef(null);

  const filtered =
    activeCategory === 'all' ? EVENTS : EVENTS.filter((e) => e.category === activeCategory);

  const focusEvent = (event) => {
    mapRef.current?.animateToRegion(
      {
        latitude: event.coordinate.latitude - 0.003,
        longitude: event.coordinate.longitude,
        latitudeDelta: 0.015,
        longitudeDelta: 0.015,
      },
      400
    );
  };

  // Navigare la EventJoinScreen cu datele evenimentului
  const openEvent = (event) => {
    navigation.navigate('EventJoin', { event });
  };

  // Navigare la CreateEventScreen
  const openCreate = () => {
    navigation.navigate('CreateEvent');
  };

  return (
    <SafeAreaView style={styles.safeArea}>
      <StatusBar style="dark" />

      <View style={styles.header}>
        <View>
          <Text style={styles.headerTitle}>SideQuest</Text>
          <View style={styles.locationRow}>
            <Text style={styles.locationPin}>📍</Text>
            <Text style={styles.locationText}>Cluj-Napoca, CJ</Text>
          </View>
        </View>
        <View style={styles.avatar}>
          <Text style={styles.avatarText}>AI</Text>
        </View>
      </View>

      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={styles.pillsContainer}
        style={styles.pillsScroll}
      >
        {CATEGORIES.map((cat) => (
          <CategoryPill
            key={cat.id}
            cat={cat}
            active={activeCategory === cat.id}
            onPress={() => setActiveCategory(cat.id)}
          />
        ))}
      </ScrollView>

      <View style={styles.mapContainer}>
        <MapView
          ref={mapRef}
          style={styles.map}
          provider={PROVIDER_DEFAULT}
          initialRegion={CLUJ_REGION}
          showsUserLocation={true}
          showsMyLocationButton={false}
          showsCompass={false}
        >
          {filtered.map((event) => (
            <CustomMarker
              key={event.id}
              event={event}
              onPress={() => {
                focusEvent(event);
                openEvent(event);   // ← tap pe marker deschide EventJoin
              }}
            />
          ))}
        </MapView>
      </View>

      <View style={styles.sheet}>
        <View style={styles.sheetHandle} />
        <View style={styles.sheetHeader}>
          <Text style={styles.sheetTitle}>🔥 Evenimente în apropiere</Text>
          <View style={styles.sheetBadge}>
            <Text style={styles.sheetBadgeText}>{filtered.length}</Text>
          </View>
        </View>
        <ScrollView
          horizontal
          showsHorizontalScrollIndicator={false}
          contentContainerStyle={styles.eventsScroll}
        >
          {filtered.map((event) => (
            <EventCard
              key={event.id}
              event={event}
              onPress={() => openEvent(event)}   // ← tap pe card deschide EventJoin
            />
          ))}
        </ScrollView>
      </View>

      <View style={styles.bottomNav}>
        <TouchableOpacity style={styles.navItem} activeOpacity={0.7}>
          <Text style={styles.navIcon}>🗺️</Text>
          <Text style={[styles.navLabel, styles.navLabelActive]}>Hartă</Text>
        </TouchableOpacity>

        {/* Butonul + deschide CreateEvent */}
        <TouchableOpacity style={styles.fab} activeOpacity={0.85} onPress={openCreate}>
          <Text style={styles.fabText}>+</Text>
        </TouchableOpacity>

        <TouchableOpacity style={styles.navItem} activeOpacity={0.7}>
          <Text style={styles.navIcon}>💬</Text>
          <Text style={styles.navLabel}>Chat</Text>
        </TouchableOpacity>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safeArea: { flex: 1, backgroundColor: '#FEF0E8' },
  header: {
    backgroundColor: CORAL,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingHorizontal: 20,
    paddingTop: 8,
    paddingBottom: 12,
  },
  headerTitle: { fontSize: 22, fontWeight: '900', color: 'white', letterSpacing: -0.5 },
  locationRow: { flexDirection: 'row', alignItems: 'center', marginTop: 1 },
  locationPin: { fontSize: 11 },
  locationText: { fontSize: 12, color: 'rgba(255,255,255,0.85)', marginLeft: 3, fontWeight: '600' },
  avatar: {
    width: 38, height: 38, borderRadius: 19,
    backgroundColor: 'rgba(255,255,255,0.25)',
    alignItems: 'center', justifyContent: 'center',
    borderWidth: 2, borderColor: 'rgba(255,255,255,0.5)',
  },
  avatarText: { color: 'white', fontWeight: '800', fontSize: 13 },
  pillsScroll: {
    backgroundColor: 'white', maxHeight: 50,
    shadowColor: '#000', shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.05, shadowRadius: 4, elevation: 3,
  },
  pillsContainer: { paddingHorizontal: 14, paddingVertical: 8, gap: 8, flexDirection: 'row', alignItems: 'center' },
  pill: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 12, paddingVertical: 5, borderRadius: 50, backgroundColor: '#F5F5F5', gap: 4 },
  pillActive: { backgroundColor: CORAL },
  pillEmoji: { fontSize: 12 },
  pillText: { fontSize: 12, fontWeight: '700', color: '#888' },
  pillTextActive: { color: 'white' },
  mapContainer: { flex: 1 },
  map: { flex: 1 },
  markerBubble: {
    paddingHorizontal: 10, paddingVertical: 5, borderRadius: 20,
    alignItems: 'center', justifyContent: 'center',
    shadowColor: '#000', shadowOffset: { width: 0, height: 3 },
    shadowOpacity: 0.2, shadowRadius: 5, elevation: 4,
  },
  markerText: { color: 'white', fontWeight: '800', fontSize: 11 },
  markerTail: {
    width: 0, height: 0,
    borderLeftWidth: 5, borderRightWidth: 5, borderTopWidth: 6,
    borderLeftColor: 'transparent', borderRightColor: 'transparent',
    alignSelf: 'center',
  },
  sheet: {
    backgroundColor: 'white', borderTopLeftRadius: 22, borderTopRightRadius: 22,
    paddingTop: 8, paddingBottom: 4,
    shadowColor: '#000', shadowOffset: { width: 0, height: -4 },
    shadowOpacity: 0.08, shadowRadius: 12, elevation: 10,
  },
  sheetHandle: { width: 36, height: 4, backgroundColor: '#E0E0E0', borderRadius: 2, alignSelf: 'center', marginBottom: 10 },
  sheetHeader: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingHorizontal: 18, marginBottom: 10 },
  sheetTitle: { fontSize: 14, fontWeight: '800', color: '#2D2D2D' },
  sheetBadge: { backgroundColor: CORAL, borderRadius: 10, paddingHorizontal: 8, paddingVertical: 2 },
  sheetBadgeText: { color: 'white', fontSize: 11, fontWeight: '800' },
  eventsScroll: { paddingHorizontal: 14, paddingBottom: 12, gap: 10, flexDirection: 'row' },
  eventCard: {
    flexDirection: 'row', alignItems: 'center',
    backgroundColor: '#FFF8F2', borderRadius: 14, padding: 10, width: 220, gap: 10,
    borderWidth: 1, borderColor: '#F0EDE9',
  },
  eventIcon: { width: 40, height: 40, borderRadius: 12, alignItems: 'center', justifyContent: 'center' },
  eventIconText: { fontSize: 18 },
  eventInfo: { flex: 1 },
  eventTitle: { fontSize: 12, fontWeight: '800', color: '#2D2D2D', marginBottom: 2 },
  eventMeta: { fontSize: 10, color: '#AAA', fontWeight: '600' },
  eventBadge: { width: 24, height: 24, borderRadius: 12, alignItems: 'center', justifyContent: 'center' },
  eventBadgeText: { color: 'white', fontWeight: '900', fontSize: 14, lineHeight: 18 },
  bottomNav: {
    flexDirection: 'row', alignItems: 'center', justifyContent: 'space-around',
    backgroundColor: 'white', paddingVertical: 10,
    paddingBottom: Platform.OS === 'ios' ? 20 : 10,
    borderTopWidth: 1, borderTopColor: '#F5F5F5',
  },
  navItem: { alignItems: 'center', gap: 2, minWidth: 60 },
  navIcon: { fontSize: 22 },
  navLabel: { fontSize: 10, color: '#CCC', fontWeight: '700' },
  navLabelActive: { color: CORAL },
  fab: {
    width: 54, height: 54, borderRadius: 27, backgroundColor: CORAL,
    alignItems: 'center', justifyContent: 'center', marginBottom: 8,
    shadowColor: CORAL, shadowOffset: { width: 0, height: 6 },
    shadowOpacity: 0.4, shadowRadius: 10, elevation: 8,
  },
  fabText: { color: 'white', fontSize: 28, fontWeight: '300', lineHeight: 32 },
});
