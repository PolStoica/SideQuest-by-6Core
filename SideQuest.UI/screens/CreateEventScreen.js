import {
  StyleSheet, Text, View, TouchableOpacity, SafeAreaView,
  ScrollView, TextInput, Alert,
} from 'react-native';
import { useState } from 'react';
import { StatusBar } from 'expo-status-bar';

const CORAL = '#E8533A';

const CATEGORIES = [
  { id: 'music',  label: 'Muzică',  emoji: '🎵', color: '#F4845F', bg: '#FDE8E0' },
  { id: 'sport',  label: 'Sport',   emoji: '⚽', color: '#4CAF82', bg: '#E0F5EC' },
  { id: 'art',    label: 'Artă',    emoji: '🎨', color: '#9B6FD4', bg: '#EFE5FF' },
  { id: 'gaming', label: 'Gaming',  emoji: '🎮', color: '#6B7280', bg: '#F3F4F6' },
  { id: 'food',   label: 'Food',    emoji: '🍕', color: '#F4A940', bg: '#FEF3E0' },
  { id: 'culture',label: 'Cultură', emoji: '🏛️', color: '#4DAEE8', bg: '#E0F2FF' },
];

export default function CreateEventScreen({ navigation }) {
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [title, setTitle] = useState('');
  const [date, setDate] = useState('');
  const [time, setTime] = useState('');
  const [maxParticipants, setMaxParticipants] = useState('');
  const [location, setLocation] = useState('');
  // 3 intrebari personalizate de la organizator
  const [questions, setQuestions] = useState(['', '', '']);

  const setQuestion = (index, val) => {
    const updated = [...questions];
    updated[index] = val;
    setQuestions(updated);
  };

  const canPublish =
    selectedCategory !== null &&
    title.trim().length > 0 &&
    date.trim().length > 0 &&
    time.trim().length > 0 &&
    maxParticipants.trim().length > 0;

  const handlePublish = () => {
    Alert.alert('Eveniment publicat! 🎉', 'Evenimentul tău este acum vizibil pe hartă.', [
      { text: 'Super!', onPress: () => navigation.goBack() },
    ]);
  };

  return (
    <SafeAreaView style={styles.safeArea}>
      <StatusBar style="dark" />

      {/* Header */}
      <View style={styles.header}>
        <TouchableOpacity onPress={() => navigation.goBack()} style={styles.backBtn} activeOpacity={0.7}>
          <Text style={styles.backArrow}>←</Text>
        </TouchableOpacity>
        <Text style={styles.headerTitle}>Creează eveniment</Text>
        <View style={{ width: 36 }} />
      </View>

      <ScrollView
        style={styles.scroll}
        contentContainerStyle={styles.scrollContent}
        showsVerticalScrollIndicator={false}
        keyboardShouldPersistTaps="handled"
      >
        {/* Categorie */}
        <Text style={styles.sectionLabel}>CATEGORIE</Text>
        <View style={styles.categoryGrid}>
          {CATEGORIES.map((cat) => {
            const isSelected = selectedCategory === cat.id;
            return (
              <TouchableOpacity
                key={cat.id}
                style={[
                  styles.categoryCard,
                  isSelected && { borderColor: cat.color, borderWidth: 2.5, backgroundColor: cat.bg },
                ]}
                onPress={() => setSelectedCategory(cat.id)}
                activeOpacity={0.8}
              >
                {isSelected && (
                  <View style={[styles.checkBadge, { backgroundColor: cat.color }]}>
                    <Text style={styles.checkBadgeText}>✓</Text>
                  </View>
                )}
                <Text style={styles.categoryEmoji}>{cat.emoji}</Text>
                <Text style={[styles.categoryLabel, isSelected && { color: cat.color, fontWeight: '800' }]}>
                  {cat.label}
                </Text>
              </TouchableOpacity>
            );
          })}
        </View>

        {/* Locatie */}
        <Text style={styles.sectionLabel}>LOCAȚIE</Text>
        <TouchableOpacity
          style={styles.locationCard}
          activeOpacity={0.8}
          onPress={() => Alert.alert('Hartă', 'Integrarea cu harta vine în curând!')}
        >
          <Text style={styles.locationPin}>📍</Text>
          <Text style={styles.locationText}>
            {location.length > 0 ? location : 'Atinge pentru a alege pe hartă'}
          </Text>
        </TouchableOpacity>

        {/* Detalii */}
        <Text style={styles.sectionLabel}>DETALII</Text>
        <View style={styles.detailsCard}>
          <TextInput
            style={styles.detailInput}
            placeholder="Titlu eveniment"
            placeholderTextColor="#C5B8B3"
            value={title}
            onChangeText={setTitle}
          />
          <View style={styles.divider} />

          <View style={styles.rowInputs}>
            <TextInput
              style={[styles.detailInput, styles.halfInput]}
              placeholder="📅  Dată"
              placeholderTextColor="#C5B8B3"
              value={date}
              onChangeText={setDate}
            />
            <View style={styles.verticalDivider} />
            <TextInput
              style={[styles.detailInput, styles.halfInput]}
              placeholder="🕐  Ora"
              placeholderTextColor="#C5B8B3"
              value={time}
              onChangeText={setTime}
            />
          </View>
          <View style={styles.divider} />

          <TextInput
            style={styles.detailInput}
            placeholder="👥  Max. participanți"
            placeholderTextColor="#C5B8B3"
            value={maxParticipants}
            onChangeText={setMaxParticipants}
            keyboardType="numeric"
          />
        </View>

        {/* Intrebari pentru participanti */}
        <Text style={styles.sectionLabel}>ÎNTREBĂRI PENTRU PARTICIPANȚI</Text>
        <View style={styles.questionsCard}>
          {questions.map((q, index) => (
            <View key={index}>
              <View style={styles.questionRow}>
                <View style={styles.questionNumber}>
                  <Text style={styles.questionNumberText}>{index + 1}</Text>
                </View>
                <TextInput
                  style={styles.questionInput}
                  placeholder={
                    index === 0 ? 'Ex: Care este nivelul tău?' :
                    index === 1 ? 'Ex: Ai echipament propriu?' :
                    'Ex: Altceva de menționat...'
                  }
                  placeholderTextColor="#C5B8B3"
                  value={q}
                  onChangeText={(val) => setQuestion(index, val)}
                />
              </View>
              {index < questions.length - 1 && <View style={styles.divider} />}
            </View>
          ))}
        </View>

        <Text style={styles.hint}>
          💡 Participanții vor răspunde la aceste întrebări înainte să se alăture.
        </Text>

        <View style={{ height: 20 }} />
      </ScrollView>

      {/* Buton fix jos */}
      <View style={styles.footer}>
        <TouchableOpacity
          style={[styles.publishBtn, !canPublish && styles.publishBtnDisabled]}
          onPress={canPublish ? handlePublish : null}
          activeOpacity={canPublish ? 0.85 : 1}
        >
          <Text style={styles.publishText}>Publică evenimentul</Text>
        </TouchableOpacity>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safeArea: { flex: 1, backgroundColor: '#F7F3F0' },

  // Header
  header: {
    backgroundColor: CORAL,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingHorizontal: 16,
    paddingVertical: 14,
  },
  backBtn: { width: 36, height: 36, alignItems: 'center', justifyContent: 'center' },
  backArrow: { color: 'white', fontSize: 22, fontWeight: '700' },
  headerTitle: { color: 'white', fontSize: 18, fontWeight: '800' },

  scroll: { flex: 1 },
  scrollContent: { paddingHorizontal: 18, paddingTop: 20, paddingBottom: 8 },

  sectionLabel: {
    fontSize: 11,
    fontWeight: '700',
    color: '#B0A09A',
    letterSpacing: 1.5,
    marginBottom: 10,
    marginTop: 8,
  },

  // Categorii
  categoryGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 10,
    marginBottom: 20,
  },
  categoryCard: {
    width: '47%',
    backgroundColor: 'white',
    borderRadius: 16,
    padding: 16,
    alignItems: 'center',
    gap: 6,
    borderWidth: 2,
    borderColor: 'transparent',
    position: 'relative',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.06,
    shadowRadius: 6,
    elevation: 2,
  },
  checkBadge: {
    position: 'absolute',
    top: 8,
    right: 8,
    width: 20,
    height: 20,
    borderRadius: 10,
    alignItems: 'center',
    justifyContent: 'center',
  },
  checkBadgeText: { color: 'white', fontSize: 11, fontWeight: '900' },
  categoryEmoji: { fontSize: 28 },
  categoryLabel: { fontSize: 14, fontWeight: '700', color: '#4A4A4A' },

  // Locatie
  locationCard: {
    backgroundColor: '#F0FDF4',
    borderRadius: 14,
    padding: 16,
    flexDirection: 'row',
    alignItems: 'center',
    gap: 10,
    marginBottom: 20,
    borderWidth: 1,
    borderColor: '#C6F0D4',
  },
  locationPin: { fontSize: 18 },
  locationText: { fontSize: 14, color: '#4CAF82', fontWeight: '600', flex: 1 },

  // Detalii
  detailsCard: {
    backgroundColor: 'white',
    borderRadius: 16,
    overflow: 'hidden',
    marginBottom: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.06,
    shadowRadius: 8,
    elevation: 2,
  },
  detailInput: {
    paddingHorizontal: 16,
    paddingVertical: 16,
    fontSize: 15,
    color: '#1A1A2E',
    flex: 1,
  },
  rowInputs: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  halfInput: { flex: 1 },
  divider: { height: 1, backgroundColor: '#F0EDE9', marginHorizontal: 16 },
  verticalDivider: { width: 1, height: 40, backgroundColor: '#F0EDE9' },

  // Intrebari
  questionsCard: {
    backgroundColor: 'white',
    borderRadius: 16,
    overflow: 'hidden',
    marginBottom: 12,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.06,
    shadowRadius: 8,
    elevation: 2,
  },
  questionRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 14,
    paddingVertical: 4,
    gap: 10,
  },
  questionNumber: {
    width: 24,
    height: 24,
    borderRadius: 12,
    backgroundColor: CORAL,
    alignItems: 'center',
    justifyContent: 'center',
    flexShrink: 0,
  },
  questionNumberText: { color: 'white', fontSize: 12, fontWeight: '800' },
  questionInput: {
    flex: 1,
    paddingVertical: 16,
    fontSize: 14,
    color: '#1A1A2E',
  },

  hint: {
    fontSize: 12,
    color: '#B0A09A',
    textAlign: 'center',
    lineHeight: 18,
  },

  // Footer
  footer: {
    backgroundColor: 'white',
    paddingHorizontal: 18,
    paddingTop: 12,
    paddingBottom: 28,
    borderTopWidth: 1,
    borderTopColor: '#F0EDE9',
  },
  publishBtn: {
    backgroundColor: CORAL,
    borderRadius: 50,
    paddingVertical: 18,
    alignItems: 'center',
    shadowColor: CORAL,
    shadowOffset: { width: 0, height: 6 },
    shadowOpacity: 0.35,
    shadowRadius: 14,
    elevation: 6,
  },
  publishBtnDisabled: {
    backgroundColor: '#E0C5BE',
    shadowOpacity: 0,
    elevation: 0,
  },
  publishText: { color: 'white', fontSize: 17, fontWeight: '800', letterSpacing: 0.3 },
});
