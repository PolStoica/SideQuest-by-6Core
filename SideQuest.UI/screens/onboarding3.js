import { StyleSheet, Text, View, TouchableOpacity, SafeAreaView, ScrollView } from 'react-native';
import { useState } from 'react';
import { StatusBar } from 'expo-status-bar';

const CORAL = '#E8533A';

const CATEGORIES = [
  { id: 1, label: 'Muzică',   emoji: '🎵', color: '#F4845F', colorLight: '#FDE8E0' },
  { id: 2, label: 'Sport',    emoji: '⚽', color: '#4CAF82', colorLight: '#E0F5EC' },
  { id: 3, label: 'Artă',     emoji: '🎨', color: '#9B6FD4', colorLight: '#EFE5FF' },
  { id: 4, label: 'Food',     emoji: '🍜', color: '#F4A940', colorLight: '#FEF3E0' },
  { id: 5, label: 'Gaming',   emoji: '🎮', color: '#6B7280', colorLight: '#F3F4F6' },
  { id: 6, label: 'Cultură',  emoji: '🏛️', color: '#6B7280', colorLight: '#F3F4F6' },
  { id: 7, label: 'Natură',   emoji: '🌿', color: '#4CAF82', colorLight: '#E0F5EC' },
  { id: 8, label: 'Dans',     emoji: '💃', color: '#F4845F', colorLight: '#FDE8E0' },
  { id: 9, label: 'Teatru',   emoji: '🎭', color: '#9B6FD4', colorLight: '#EFE5FF' },
];

export default function onboarding3({ navigation }) {
  const [selected, setSelected] = useState([]);

  function toggle(id) {
    setSelected(prev =>
      prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id]
    );
  }

  const canContinue = selected.length >= 3;

  // grupeaza categoriile cate 2 pe rand
  const rows = [];
  for (let i = 0; i < CATEGORIES.length; i += 2) {
    rows.push(CATEGORIES.slice(i, i + 2));
  }

  return (
    <SafeAreaView style={styles.safeArea}>
      <StatusBar style="dark" />

      {/* Header roz */}
      <View style={styles.header}>
        <Text style={styles.step}>PASUL 2 DIN 3</Text>
        <Text style={styles.title}>Ce îți place{'\n'}să explorezi?</Text>
        <Text style={styles.subtitle}>Alege cel puțin 3 categorii</Text>
        {/* Blob decorativ */}
        <View style={styles.blob} />
      </View>

      {/* Val curb */}
      <View style={styles.waveContainer}>
        <View style={styles.waveCurve} />
      </View>

      {/* Lista categorii */}
      <ScrollView
        style={styles.scroll}
        contentContainerStyle={styles.scrollContent}
        showsVerticalScrollIndicator={false}
      >
        {rows.map((row, rowIndex) => (
          <View key={rowIndex} style={styles.row}>
            {row.map(cat => {
              const isSelected = selected.includes(cat.id);
              return (
                <TouchableOpacity
                  key={cat.id}
                  style={[
                    styles.chip,
                    isSelected
                      ? { backgroundColor: cat.color }
                      : { backgroundColor: cat.colorLight }
                  ]}
                  onPress={() => toggle(cat.id)}
                  activeOpacity={0.8}
                >
                  <Text style={styles.chipEmoji}>{cat.emoji}</Text>
                  <Text style={[
                    styles.chipLabel,
                    { color: isSelected ? '#FFFFFF' : '#4A4A4A' }
                  ]}>
                    {cat.label}
                  </Text>
                </TouchableOpacity>
              );
            })}
          </View>
        ))}

        {/* Counter selectate */}
        <Text style={styles.counter}>
          {selected.length > 0
            ? `${selected.length} selectate ✓`
            : 'Nicio categorie selectată'}
        </Text>
      </ScrollView>

      {/* Buton continua */}
      <View style={styles.footer}>
        <TouchableOpacity
          style={[styles.ctaButton, !canContinue && styles.ctaDisabled]}
          activeOpacity={canContinue ? 0.85 : 1}
          onPress={() => canContinue && navigation.navigate('onboarding4')}
        >
          <Text style={styles.ctaText}>Continuă →</Text>
        </TouchableOpacity>
      </View>

      {/* Dots */}
      <View style={styles.dotsContainer}>
        <View style={styles.dot} />
        <View style={styles.dot} />
        <View style={[styles.dot, styles.dotActive]} />
      </View>

    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safeArea: {
    flex: 1,
    backgroundColor: '#FEF0E8',
  },

  // Header
  header: {
    backgroundColor: '#FADADD',
    paddingTop: 28,
    paddingBottom: 48,
    paddingHorizontal: 28,
    overflow: 'hidden',
  },
  blob: {
    position: 'absolute',
    top: -30,
    right: -30,
    width: 110,
    height: 110,
    borderRadius: 55,
    backgroundColor: '#F9B8C4',
    opacity: 0.5,
  },
  step: {
    fontSize: 11,
    fontWeight: '700',
    color: CORAL,
    letterSpacing: 2,
    marginBottom: 8,
  },
  title: {
    fontSize: 30,
    fontWeight: '800',
    color: '#1A1A2E',
    lineHeight: 38,
  },
  subtitle: {
    fontSize: 14,
    color: '#9A8C88',
    marginTop: 8,
  },

  // Val
  waveContainer: {
    height: 36,
    backgroundColor: '#FADADD',
  },
  waveCurve: {
    height: 36,
    backgroundColor: '#FEF0E8',
    borderTopLeftRadius: 36,
    borderTopRightRadius: 36,
  },

  // Scroll
  scroll: {
    flex: 1,
  },
  scrollContent: {
    paddingHorizontal: 20,
    paddingTop: 12,
    paddingBottom: 8,
    gap: 12,
  },

  // Randuri si chipuri
  row: {
    flexDirection: 'row',
    gap: 12,
  },
  chip: {
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    gap: 10,
    paddingVertical: 16,
    paddingHorizontal: 18,
    borderRadius: 16,
  },
  chipEmoji: {
    fontSize: 22,
  },
  chipLabel: {
    fontSize: 16,
    fontWeight: '600',
  },

  // Counter
  counter: {
    textAlign: 'center',
    fontSize: 14,
    color: CORAL,
    fontWeight: '600',
    marginTop: 8,
    marginBottom: 4,
  },

  // Footer
  footer: {
    paddingHorizontal: 24,
    paddingBottom: 8,
    paddingTop: 8,
  },
  ctaButton: {
    backgroundColor: CORAL,
    width: '100%',
    paddingVertical: 18,
    borderRadius: 50,
    alignItems: 'center',
    shadowColor: CORAL,
    shadowOffset: { width: 0, height: 6 },
    shadowOpacity: 0.35,
    shadowRadius: 14,
    elevation: 6,
  },
  ctaDisabled: {
    backgroundColor: '#E0C5BE',
    shadowOpacity: 0,
    elevation: 0,
  },
  ctaText: {
    color: '#FFFFFF',
    fontSize: 17,
    fontWeight: '700',
    letterSpacing: 0.3,
  },

  // Dots
  dotsContainer: {
    flexDirection: 'row',
    gap: 6,
    justifyContent: 'center',
    paddingVertical: 16,
  },
  dot: {
    width: 6,
    height: 6,
    borderRadius: 3,
    backgroundColor: '#D9C5BC',
  },
  dotActive: {
    width: 20,
    backgroundColor: CORAL,
  },
});