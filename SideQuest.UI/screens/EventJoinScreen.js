import {
  StyleSheet, Text, View, TouchableOpacity, SafeAreaView,
  ScrollView, TextInput, Alert,
} from 'react-native';
import { useState } from 'react';
import { StatusBar } from 'expo-status-bar';

const CORAL = '#E8533A';

// Intrebari automate pe categorie
const AUTO_QUESTIONS = {
  music: [
    { id: 'a1', question: 'Ai mai participat la un concert în aer liber?', type: 'yesno' },
    { id: 'a2', question: 'Vei veni însoțit de cineva?', type: 'text', placeholder: 'Ex: Vin cu 2 prieteni...' },
    { id: 'a3', question: 'Ce gen muzical preferi?', type: 'text', placeholder: 'Ex: Rock, Pop, Jazz...' },
  ],
  sport: [
    { id: 'a1', question: 'Care este nivelul tău de experiență?', type: 'choice', options: ['Începător', 'Intermediar', 'Avansat'] },
    { id: 'a2', question: 'Ai echipament propriu?', type: 'yesno' },
    { id: 'a3', question: 'Vei veni însoțit de cineva?', type: 'text', placeholder: 'Ex: Vin singur / cu un prieten...' },
  ],
  art: [
    { id: 'a1', question: 'Ai mai participat la un atelier similar?', type: 'yesno' },
    { id: 'a2', question: 'Care este nivelul tău?', type: 'choice', options: ['Începător', 'Intermediar', 'Avansat'] },
    { id: 'a3', question: 'Vei veni însoțit de cineva?', type: 'text', placeholder: 'Ex: Vin singur...' },
  ],
  food: [
    { id: 'a1', question: 'Ai alergii sau restricții alimentare?', type: 'yesno' },
    { id: 'a2', question: 'Dacă da, care sunt?', type: 'text', placeholder: 'Ex: Fără gluten, vegetarian...' },
    { id: 'a3', question: 'Vei veni însoțit de cineva?', type: 'text', placeholder: 'Ex: Vin cu familia...' },
  ],
  gaming: [
    { id: 'a1', question: 'Ai mai participat la un turneu?', type: 'yesno' },
    { id: 'a2', question: 'Pe ce platformă joci cel mai mult?', type: 'choice', options: ['PC', 'Console', 'Mobile'] },
    { id: 'a3', question: 'Vei veni însoțit de cineva?', type: 'text', placeholder: 'Ex: Vin cu un coechipier...' },
  ],
  default: [
    { id: 'a1', question: 'Ai mai participat la evenimente similare?', type: 'yesno' },
    { id: 'a2', question: 'Vei veni însoțit de cineva?', type: 'text', placeholder: 'Ex: Vin singur / cu prieteni...' },
    { id: 'a3', question: 'Ce așteptări ai de la acest eveniment?', type: 'text', placeholder: 'Spune-ne câteva cuvinte...' },
  ],
};

// Avatare mock pentru locuri
const AVATARS = [
  { id: 1, color: '#E8533A', label: 'AI' },
  { id: 2, color: '#9B6FD4', label: 'MR' },
  { id: 3, color: '#4CAF82', label: 'DP' },
];

// ─── Sub-componente ───────────────────────────────────────────────────────────

function YesNoQuestion({ question, value, onChange }) {
  return (
    <View style={styles.questionCard}>
      <Text style={styles.questionTag}>Întrebare automată de la aplicație</Text>
      <Text style={styles.questionText}>{question}</Text>
      <View style={styles.yesNoRow}>
        <TouchableOpacity
          style={[styles.yesNoBtn, value === true && styles.yesNoBtnActive]}
          onPress={() => onChange(true)}
          activeOpacity={0.8}
        >
          {value === true && <Text style={styles.checkMark}>✓ </Text>}
          <Text style={[styles.yesNoLabel, value === true && styles.yesNoLabelActive]}>Da</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.yesNoBtn, value === false && styles.yesNoBtnActiveNo]}
          onPress={() => onChange(false)}
          activeOpacity={0.8}
        >
          <Text style={[styles.yesNoLabel, value === false && styles.yesNoLabelActive]}>Nu</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}

function TextQuestion({ question, placeholder, value, onChange }) {
  return (
    <View style={styles.questionCard}>
      <Text style={styles.questionTag}>Întrebare automată de la aplicație</Text>
      <Text style={styles.questionText}>{question}</Text>
      <TextInput
        style={styles.questionInput}
        placeholder={placeholder}
        placeholderTextColor="#C5B8B3"
        value={value}
        onChangeText={onChange}
        multiline
      />
    </View>
  );
}

function ChoiceQuestion({ question, options, value, onChange }) {
  return (
    <View style={styles.questionCard}>
      <Text style={styles.questionTag}>Întrebare automată de la aplicație</Text>
      <Text style={styles.questionText}>{question}</Text>
      <View style={styles.choiceRow}>
        {options.map((opt) => (
          <TouchableOpacity
            key={opt}
            style={[styles.choiceBtn, value === opt && styles.choiceBtnActive]}
            onPress={() => onChange(opt)}
            activeOpacity={0.8}
          >
            <Text style={[styles.choiceLabel, value === opt && styles.choiceLabelActive]}>
              {opt}
            </Text>
          </TouchableOpacity>
        ))}
      </View>
    </View>
  );
}

function CustomQuestion({ question, index, value, onChange }) {
  return (
    <View style={styles.questionCardCustom}>
      <Text style={styles.questionTagCustom}>Întrebare custom</Text>
      <Text style={styles.questionText}>{question}</Text>
      <TextInput
        style={styles.questionInput}
        placeholder="Răspunsul tău..."
        placeholderTextColor="#C5B8B3"
        value={value}
        onChangeText={onChange}
        multiline
      />
    </View>
  );
}

// ─── Screen principal ─────────────────────────────────────────────────────────
export default function EventJoinScreen({ navigation, route }) {
  // Primesti evenimentul prin route.params de la MapScreen
  // Ex: navigation.navigate('EventJoin', { event: eventObj })
  const event = route?.params?.event ?? {
    id: 1,
    title: 'Concert în Parc',
    category: 'music',
    emoji: '🎵',
    color: '#E8533A',
    date: 'Azi 20:00',
    location: 'Parcul Central',
    spots: 18,
    total: 30,
  };

  const questions = AUTO_QUESTIONS[event.category] ?? AUTO_QUESTIONS.default;

  // Raspunsuri pentru intrebarile automate
  const [autoAnswers, setAutoAnswers] = useState({});
  // Raspunsuri pentru intrebarile custom ale organizatorului
  const [customAnswers, setCustomAnswers] = useState({});

  // Intrebarile custom ale organizatorului (in practica vin din backend)
  const customQuestions = event.customQuestions ?? [
    { id: 'c1', question: 'Care este genul tău muzical preferat?' },
  ];

  const setAuto = (id, val) => setAutoAnswers((prev) => ({ ...prev, [id]: val }));
  const setCustom = (id, val) => setCustomAnswers((prev) => ({ ...prev, [id]: val }));

  const handleSubmit = () => {
    Alert.alert('Cerere trimisă! 🎉', 'Organizatorul va răspunde în curând.', [
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
        <Text style={styles.headerTitle}>Cerere participare</Text>
        <View style={{ width: 36 }} />
      </View>

      <ScrollView
        style={styles.scroll}
        contentContainerStyle={styles.scrollContent}
        showsVerticalScrollIndicator={false}
        keyboardShouldPersistTaps="handled"
      >
        {/* Sectiunea EVENIMENT */}
        <Text style={styles.sectionLabel}>EVENIMENT</Text>
        <View style={styles.eventCard}>
          <View style={[styles.eventIconBox, { backgroundColor: event.color + '22' }]}>
            <Text style={styles.eventIconText}>{event.emoji}</Text>
          </View>
          <View style={styles.eventInfo}>
            <Text style={styles.eventTitle}>{event.title}</Text>
            <Text style={styles.eventMeta}>{event.date} · {event.location}</Text>
          </View>
        </View>

        {/* Locuri disponibile */}
        <View style={styles.spotsCard}>
          <View style={styles.spotsLeft}>
            <View style={styles.dotGreen} />
            <Text style={styles.spotsText}>
              <Text style={styles.spotsNum}>{event.spots} locuri{'\n'}</Text>
              disponibile
            </Text>
          </View>
          <View style={styles.spotsRight}>
            <View style={styles.avatarsRow}>
              {AVATARS.map((av) => (
                <View key={av.id} style={[styles.avatarSmall, { backgroundColor: av.color }]}>
                  <Text style={styles.avatarSmallText}>{av.label}</Text>
                </View>
              ))}
            </View>
            <Text style={styles.spotsCounter}>{event.total - event.spots}/{event.total}</Text>
          </View>
        </View>

        {/* Intrebari automate */}
        <Text style={styles.sectionLabel}>ÎNTREBĂRI AUTOMATE</Text>
        {questions.map((q) => {
          if (q.type === 'yesno') {
            return (
              <YesNoQuestion
                key={q.id}
                question={q.question}
                value={autoAnswers[q.id]}
                onChange={(val) => setAuto(q.id, val)}
              />
            );
          }
          if (q.type === 'choice') {
            return (
              <ChoiceQuestion
                key={q.id}
                question={q.question}
                options={q.options}
                value={autoAnswers[q.id]}
                onChange={(val) => setAuto(q.id, val)}
              />
            );
          }
          return (
            <TextQuestion
              key={q.id}
              question={q.question}
              placeholder={q.placeholder}
              value={autoAnswers[q.id] ?? ''}
              onChange={(val) => setAuto(q.id, val)}
            />
          );
        })}

        {/* Intrebari de la organizator */}
        <Text style={styles.sectionLabel}>ÎNTREBĂRI DE LA ORGANIZATOR</Text>
        {customQuestions.map((q, i) => (
          <CustomQuestion
            key={q.id}
            question={q.question}
            index={i}
            value={customAnswers[q.id] ?? ''}
            onChange={(val) => setCustom(q.id, val)}
          />
        ))}
        <TouchableOpacity style={styles.addQuestionHint} activeOpacity={0.6}>
          <Text style={styles.addQuestionText}>
            + Organizatorul poate adăuga întrebări personalizate
          </Text>
        </TouchableOpacity>

        <View style={{ height: 20 }} />
      </ScrollView>

      {/* Buton fix jos */}
      <View style={styles.footer}>
        <TouchableOpacity style={styles.submitBtn} onPress={handleSubmit} activeOpacity={0.85}>
          <Text style={styles.submitText}>Trimite cererea</Text>
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

  // Card eveniment
  eventCard: {
    backgroundColor: 'white',
    borderRadius: 16,
    padding: 14,
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
    marginBottom: 10,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.06,
    shadowRadius: 8,
    elevation: 2,
  },
  eventIconBox: {
    width: 46,
    height: 46,
    borderRadius: 14,
    alignItems: 'center',
    justifyContent: 'center',
  },
  eventIconText: { fontSize: 22 },
  eventInfo: { flex: 1 },
  eventTitle: { fontSize: 15, fontWeight: '800', color: '#1A1A2E' },
  eventMeta: { fontSize: 12, color: '#9A8C88', marginTop: 2 },

  // Locuri
  spotsCard: {
    backgroundColor: '#F0FDF4',
    borderRadius: 14,
    padding: 14,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginBottom: 20,
    borderWidth: 1,
    borderColor: '#C6F0D4',
  },
  spotsLeft: { flexDirection: 'row', alignItems: 'center', gap: 8 },
  dotGreen: { width: 10, height: 10, borderRadius: 5, backgroundColor: '#4CAF82' },
  spotsText: { fontSize: 13, color: '#2D2D2D', lineHeight: 18 },
  spotsNum: { fontWeight: '800', color: '#4CAF82' },
  spotsRight: { alignItems: 'flex-end', gap: 4 },
  avatarsRow: { flexDirection: 'row' },
  avatarSmall: {
    width: 26,
    height: 26,
    borderRadius: 13,
    alignItems: 'center',
    justifyContent: 'center',
    marginLeft: -6,
    borderWidth: 2,
    borderColor: '#F0FDF4',
  },
  avatarSmallText: { color: 'white', fontSize: 8, fontWeight: '800' },
  spotsCounter: { fontSize: 11, fontWeight: '700', color: '#9A8C88' },

  // Intrebari automate
  questionCard: {
    backgroundColor: 'white',
    borderRadius: 14,
    padding: 14,
    marginBottom: 10,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 6,
    elevation: 1,
  },
  questionTag: {
    fontSize: 10,
    fontWeight: '700',
    color: '#B0A09A',
    letterSpacing: 0.5,
    marginBottom: 5,
  },
  questionText: {
    fontSize: 14,
    fontWeight: '700',
    color: '#1A1A2E',
    marginBottom: 10,
    lineHeight: 20,
  },
  questionInput: {
    backgroundColor: '#F7F3F0',
    borderRadius: 10,
    padding: 12,
    fontSize: 14,
    color: '#1A1A2E',
    minHeight: 44,
  },

  // Yes/No
  yesNoRow: { flexDirection: 'row', gap: 10 },
  yesNoBtn: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 20,
    paddingVertical: 8,
    borderRadius: 50,
    backgroundColor: '#F0EDE9',
  },
  yesNoBtnActive: { backgroundColor: CORAL },
  yesNoBtnActiveNo: { backgroundColor: '#1A1A2E' },
  checkMark: { color: 'white', fontWeight: '800', fontSize: 13 },
  yesNoLabel: { fontSize: 14, fontWeight: '700', color: '#9A8C88' },
  yesNoLabelActive: { color: 'white' },

  // Choice
  choiceRow: { flexDirection: 'row', flexWrap: 'wrap', gap: 8 },
  choiceBtn: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 50,
    backgroundColor: '#F0EDE9',
    borderWidth: 2,
    borderColor: 'transparent',
  },
  choiceBtnActive: { backgroundColor: '#FFF0EB', borderColor: CORAL },
  choiceLabel: { fontSize: 13, fontWeight: '700', color: '#9A8C88' },
  choiceLabelActive: { color: CORAL },

  // Intrebari custom organizator
  questionCardCustom: {
    backgroundColor: 'white',
    borderRadius: 14,
    padding: 14,
    marginBottom: 10,
    borderLeftWidth: 3,
    borderLeftColor: CORAL,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 6,
    elevation: 1,
  },
  questionTagCustom: {
    fontSize: 10,
    fontWeight: '700',
    color: CORAL,
    letterSpacing: 0.5,
    marginBottom: 5,
  },

  addQuestionHint: {
    paddingVertical: 12,
    alignItems: 'center',
    borderWidth: 1.5,
    borderColor: '#F0EDE9',
    borderRadius: 12,
    borderStyle: 'dashed',
    marginTop: 4,
  },
  addQuestionText: {
    fontSize: 12,
    color: CORAL,
    fontWeight: '600',
  },

  // Footer buton
  footer: {
    backgroundColor: 'white',
    paddingHorizontal: 18,
    paddingTop: 12,
    paddingBottom: 28,
    borderTopWidth: 1,
    borderTopColor: '#F0EDE9',
  },
  submitBtn: {
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
  submitText: { color: 'white', fontSize: 17, fontWeight: '800', letterSpacing: 0.3 },
});
