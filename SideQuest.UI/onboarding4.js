import { StyleSheet, Text, View, TextInput, TouchableOpacity, SafeAreaView } from 'react-native';
import { useState } from 'react';
import { StatusBar } from 'expo-status-bar';

const CORAL = '#E8533A';

export default function onboarding4({ navigation }) {
  const [email, setEmail] = useState('');
  const [parola, setParola] = useState('');
  const [confirmaParola, setConfirmaParola] = useState('');

  const canContinue = email.length > 0 && parola.length > 0 && confirmaParola.length > 0;

  return (
    <SafeAreaView style={styles.safeArea}>
      <StatusBar style="dark" />

      {/* Header */}
      <View style={styles.header}>
        <Text style={styles.step}>PASUL 3 DIN 3</Text>
        <Text style={styles.title}>Aproape acolo!{'\n'}Creează-ți contul</Text>
        <Text style={styles.subtitle}>Îți promitem, durează 30s</Text>
        <View style={styles.blob} />
      </View>

      {/* Val curb */}
      <View style={styles.waveContainer}>
        <View style={styles.waveCurve} />
      </View>

      {/* Formular */}
      <View style={styles.form}>

        <Text style={styles.label}>EMAIL</Text>
        <TextInput
          style={styles.input}
          placeholder="adresa@email.com"
          placeholderTextColor="#C5B8B3"
          value={email}
          onChangeText={setEmail}
          keyboardType="email-address"
          autoCapitalize="none"
        />

        <Text style={styles.label}>PAROLĂ</Text>
        <TextInput
          style={styles.input}
          placeholder="••••••••"
          placeholderTextColor="#C5B8B3"
          value={parola}
          onChangeText={setParola}
          secureTextEntry
        />

        <Text style={styles.label}>CONFIRMĂ PAROLA</Text>
        <TextInput
          style={styles.input}
          placeholder="••••••••"
          placeholderTextColor="#C5B8B3"
          value={confirmaParola}
          onChangeText={setConfirmaParola}
          secureTextEntry
        />

        {/* Buton */}
        <TouchableOpacity
          style={[styles.ctaButton, !canContinue && styles.ctaDisabled]}
          activeOpacity={canContinue ? 0.85 : 1}
          onPress={() => canContinue && navigation.navigate('Home')}
        >
          <Text style={styles.ctaText}>Intru în aventură! 🗺️</Text>
        </TouchableOpacity>

        {/* Termeni */}
        <Text style={styles.terms}>
          Prin înregistrare accepți{' '}
          <Text style={styles.termsLink}>Termenii</Text>
          {' '}și{'\n'}
          <Text style={styles.termsLink}>Politica de confidențialitate</Text>
        </Text>

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

  // Form
  form: {
    flex: 1,
    paddingHorizontal: 24,
    paddingTop: 20,
  },
  label: {
    fontSize: 11,
    fontWeight: '700',
    color: '#9A8C88',
    letterSpacing: 1.5,
    marginBottom: 8,
    marginTop: 4,
  },
  input: {
    backgroundColor: '#FFFFFF',
    borderRadius: 14,
    paddingHorizontal: 16,
    paddingVertical: 16,
    fontSize: 16,
    color: '#1A1A2E',
    marginBottom: 16,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.05,
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
    marginTop: 8,
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

  // Termeni
  terms: {
    textAlign: 'center',
    fontSize: 13,
    color: '#9A8C88',
    marginTop: 16,
    lineHeight: 20,
  },
  termsLink: {
    color: '#1A1A2E',
    fontWeight: '700',
    textDecorationLine: 'underline',
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