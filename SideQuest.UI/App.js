import 'react-native-gesture-handler';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import onboarding1 from './screens/onboarding1';
import onboarding2 from './screens/onboarding2';
import onboarding3 from './screens/onboarding3';
import onboarding4 from './screens/onboarding4';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="onboarding1" component={onboarding1} />
        <Stack.Screen name="onboarding2" component={onboarding2} />
        <Stack.Screen name="onboarding3" component={onboarding3} />
        <Stack.Screen name="onboarding4" component={onboarding4} />
      </Stack.Navigator>
    </NavigationContainer>
  );
}