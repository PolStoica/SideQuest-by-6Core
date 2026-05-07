import { StyleSheet, Text, View, TextInput, TouchableOpacity, SafeAreaView } from 'react-native';
import { useState } from 'react';
import { StatusBar } from 'expo-status-bar';

const CORAL = '#E8533A';

function BackgroundDecorations() {
  return (
    <>
      <Text style={[styles.deco, { top: -2, left: 30, fontSize: 38 }]}>✦</Text>
      <Text style={[styles.deco, { top: 120, right: 80, fontSize: 22 }]}>✦</Text>
      <Text style={[styles.deco, { top: 10, right: 80, fontSize: 18 }]}>●</Text>
      <Text style={[styles.deco, { top: 140, left: 20, fontSize: 18 }]}>●</Text>
      <Text style={[styles.deco, { bottom: 220, right: 30, fontSize: 34 }]}>✦</Text>
      <Text style={[styles.deco, { bottom: 160, left: 80, fontSize: 18 }]}>●</Text>

      <View style={[styles.circle, { top: 55, right: 90, width: 10, height: 10, opacity: 0.25 }]} />
      <View style={[styles.circle, { top: 190, left: 18, width: 7, height: 7, opacity: 0.2 }]} />
      <View style={[styles.circle, { bottom: 30, right: 10, width: 15, height: 15, opacity: 0.2 }]} />

      {/* Cercuri goale (ring) */}
      <View style={[styles.ring, { top: 100, left: 220, width: 28, height: 28, opacity: 0.2 }]} />
      <View style={[styles.ring, { bottom: 10, right: 55, width: 20, height: 20, opacity: 0.15 }]} />

      {/* Blob mare decorativ in colt */}
      <View style={styles.blobTopRight} />
      <View style={styles.blobBottomLeft} />
    </>
  );
}

export default function onboarding2({ navigation }) {
  const [name, setName] = useState('');

  return (
    <SafeAreaView style={styles.safeArea}>
      <StatusBar style="dark" />

      {/* Header roz cu val */}
      <View style={styles.header}>
        <Text style={styles.step}>PASUL 1 DIN 3</Text>
        <Text style={styles.title}>Hai sa ne cunoastem!</Text>
        <Text style={styles.wave}>👋</Text>
        <BackgroundDecorations />
      </View>

      {/* Val decorativ */}
      <View style={styles.waveContainer}>
        <View style={styles.waveCurve} />
      </View>

      {/* Continut */}
      <View style={styles.content}>

        {/* Poza de profil */}
        <TouchableOpacity style={styles.photoCircle} activeOpacity={0.8}>
          <Text style={styles.cameraIcon}>📷</Text>
        </TouchableOpacity>
        <Text style={styles.photoLabel}>Adaugă o poză de profil</Text>

        {/* Input nume */}
        <Text style={styles.inputLabel}>NUMELE TĂU</Text>
        <TextInput
          style={styles.input}
          placeholder="ex: Alex"
          placeholderTextColor="#C5B8B3"
          value={name}
          onChangeText={setName}
        />

        {/* Buton */}
        <TouchableOpacity
          style={styles.ctaButton}
          activeOpacity={0.85}
          onPress={() => navigation.navigate('onboarding3')}
        >
          <Text style={styles.ctaText}>Continuă →</Text>
        </TouchableOpacity>

      </View>

      {/* Dots */}
      <View style={styles.dotsContainer}>
        <View style={styles.dot} />
        <View style={[styles.dot, styles.dotActive]} />
        <View style={styles.dot} />
      </View>

    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safeArea: {
    flex: 1,
    backgroundColor: '#FEF0E8',
  },

  //deco
   deco: {
    position: 'absolute',
    color: '#ec6767',
    fontWeight: 'bold',
  },
  circle: {
    position: 'absolute',
    borderRadius: 99,
    backgroundColor: '#E8533A',
  },
  ring: {
    position: 'absolute',
    borderRadius: 99,
    borderWidth: 2,
    borderColor: '#caad08',
    backgroundColor: 'transparent',
  },
  blobTopRight: {
    position: 'absolute',
    top: -10,
    right: -60,
    width: 140,
    height: 140,
    borderRadius: 90,
    backgroundColor: '#e65d0f',
    opacity: 0.35,
  },
  blobBottomLeft: {
    position: 'absolute',
    bottom: 50,
    left: -60,
    width: 100,
    height: 100,
    borderRadius: 100,
    backgroundColor: '#abd809',
    opacity: 0.25,
  },

  // Header
  header: {
    backgroundColor: '#FFE5DC',
    paddingTop: 30,
    paddingBottom: 50,
    paddingHorizontal: 28,
  },
  step: {
    fontSize: 11,
    fontWeight: '700',
    color: CORAL,
    letterSpacing: 2,
    marginBottom: 8,
  },
  title: {
    fontSize: 25,
    fontWeight: '800',
    color: '#1A1A2E',
  },
  wave: {
    fontSize: 38,
    marginTop: 6,
  },

  // Val curb aici merge pusa o poza sau cv
  waveContainer: {
    height: 40,
    backgroundColor: '#FFE5DC',
  },
  waveCurve: {
    height: 40,
    backgroundColor: '#FEF0E8',
    borderTopLeftRadius: 40,
    borderTopRightRadius: 40,
  },

  // Continut
  content: {
    flex: 1,
    alignItems: 'center',
    paddingHorizontal: 28,
    paddingTop: 10,
  },

  // Poza
  photoCircle: {
    width: 120,
    height: 120,
    borderRadius: 60,
    borderWidth: 3,
    borderColor: '#ffb88f',
    borderStyle: 'dashed',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#FFF8F5',
    marginBottom: 12,
  },
  cameraIcon: {
    fontSize: 40,
  },
  photoLabel: {
    fontSize: 13,
    color: CORAL,
    marginBottom: 60,
  },

  // Input
  inputLabel: {
    alignSelf: 'flex-start',
    fontSize: 13,
    fontWeight: '700',
    color: '#9A8C88',
    letterSpacing: 1.5,
    marginBottom: 8,
  },
  input: {
    width: '100%',
    backgroundColor: '#FFFFFF',
    borderRadius: 12,
    paddingHorizontal: 16,
    paddingVertical: 16,
    fontSize: 16,
    color: '#1A1A2E',
    marginBottom: 32,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 8,
    elevation: 2,
  },

  // Buton
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
    paddingBottom: 30,
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