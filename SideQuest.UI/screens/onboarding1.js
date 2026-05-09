import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, Image, TouchableOpacity, SafeAreaView, Dimensions } from 'react-native';

const { width, height } = Dimensions.get('window');

//decoratiunile stelutele si bilele
function BackgroundDecorations() {
  return (
    <>
      <Text style={[styles.deco, { top: 80, left: 30, fontSize: 38 }]}>✦</Text>
      <Text style={[styles.deco, { top: 120, right: 40, fontSize: 22 }]}>✦</Text>
      <Text style={[styles.deco, { top: 60, right: 90, fontSize: 18 }]}>●</Text>
      <Text style={[styles.deco, { top: 200, left: 20, fontSize: 18 }]}>●</Text>
      <Text style={[styles.deco, { bottom: 220, right: 30, fontSize: 34 }]}>✦</Text>
      <Text style={[styles.deco, { bottom: 160, left: 50, fontSize: 18 }]}>●</Text>

      <View style={[styles.circle, { top: 55, right: 75, width: 10, height: 10, opacity: 0.25 }]} />
      <View style={[styles.circle, { top: 190, left: 18, width: 7, height: 7, opacity: 0.2 }]} />
      <View style={[styles.circle, { bottom: 300, right: 50, width: 8, height: 8, opacity: 0.2 }]} />

      {/* Cercuri goale (ring) */}
      <View style={[styles.ring, { top: 100, left: 55, width: 28, height: 28, opacity: 0.2 }]} />
      <View style={[styles.ring, { bottom: 210, right: 55, width: 20, height: 20, opacity: 0.15 }]} />

      {/* Blob mare decorativ in colt */}
      <View style={styles.blobTopRight} />
      <View style={styles.blobBottomLeft} />
    </>
  );
}

//iconita SideQuest
function AppIcon() {
  return (
    <View style={styles.iconWrapper}>
       <Image
        source={require("../assets/SideQuestLogo.jpeg")}
        style={styles.iconImage}
        resizeMode="contain"
      />
    </View>
  );
}

function PaginationDots() {
  return (
    <View style={styles.dotsContainer}>
      <View style={[styles.dot, styles.dotActive]} />
      <View style={styles.dot} />
      <View style={styles.dot} />
    </View>
  );
}

export default function onboarding1({ navigation }) {
  return (
    <SafeAreaView style={styles.safeArea}>
      <StatusBar style="dark" />
      <View style={styles.container}>

        <BackgroundDecorations />

        {/* Icon + Titlu */}
        <View style={styles.headerSection}>
          <AppIcon />
          <Text style={styles.appName}>SideQuest</Text>
          <Text style={styles.tagline}>AVENTURA E LA UN PAS DE TINE</Text>
        </View>

        {/* Text principal */}
        <View style={styles.heroSection}>
          <Text style={styles.heroTitle}>
            Descoperă locuri{'\n'}ascunse{'\n'}și oameni fascinanți
          </Text>
          <Text style={styles.heroSubtitle}>
            Evenimente unice, experiențe{'\n'}reale, tot ce contează.
          </Text>
        </View>

        {/* Butoane */}
        <View style={styles.actionsSection}>
          <TouchableOpacity style={styles.ctaButton} activeOpacity={0.85} onPress={() => navigation.navigate('onboarding2')}>
            <Text style={styles.ctaText}>Să începem →</Text>
          </TouchableOpacity>

          <TouchableOpacity activeOpacity={0.7}>
            <Text style={styles.loginText}>
              Ai deja cont?{' '}
              <Text style={styles.loginLink}>Autentifică-te</Text>
            </Text>
          </TouchableOpacity>
        </View>

        <PaginationDots />

      </View>
    </SafeAreaView>
  );
}

const CORAL = '#E8533A';

const styles = StyleSheet.create({
  safeArea: {
    flex: 1,
    backgroundColor: '#FEF0E8',
  },
  container: {
    flex: 1,
    backgroundColor: '#FFE5DC',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 20,
    paddingHorizontal: 30,
  },

  // Decoratiuni
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
    top: -60,
    right: -60,
    width: 180,
    height: 180,
    borderRadius: 90,
    backgroundColor: '#e65d0f',
    opacity: 0.35,
  },
  blobBottomLeft: {
    position: 'absolute',
    bottom: -80,
    left: -60,
    width: 200,
    height: 200,
    borderRadius: 100,
    backgroundColor: '#abd809',
    opacity: 0.25,
  },

  // Header
  headerSection: {
    alignItems: 'center',
    marginTop: 20,
  },
  iconWrapper: {
    width: 120,
    height: 120,
    borderRadius: 35,
    backgroundColor: '#FFFFFF',
    overflow: 'hidden',
    alignItems: 'center',
    justifyContent: 'center',
    elevation: 4,
    marginBottom: 10,
  },
  iconImage: {
    width: 160,
    height: 160,
  },
  appName: {
    fontSize: 35,
    fontWeight: '900',
    color: '#d30000',
    letterSpacing: 0.9,
  },
  tagline: {
    fontSize: 12,
    fontWeight: '600',
    color: CORAL,
    letterSpacing: 1.5,
    marginTop: 2,
  },

  // Hero = mainText
  heroSection: {
    alignItems: 'center',
    paddingHorizontal: 10,
  },
  heroTitle: {
    fontSize: 30,
    fontWeight: '800',
    color: '#191933',
    textAlign: 'center',
    lineHeight: 38,
    marginBottom: 12,
  },
  heroSubtitle: {
    fontSize: 15,
    color: '#9A8C88',
    textAlign: 'center',
    lineHeight: 20,
  },

  // Actiuni
  actionsSection: {
    alignItems: 'center',
    width: '100%',
    gap: 16,
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
  ctaText: {
    color: '#FFFFFF',
    fontSize: 17,
    fontWeight: '700',
    letterSpacing: 0.3,
  },
  loginText: {
    fontSize: 14,
    color: '#9A8C88',
  },
  loginLink: {
    color: '#1A1A2E',
    fontWeight: '600',
  },

  // Dots cum se comuta pg
  dotsContainer: {
    flexDirection: 'row',
    gap: 6,
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
