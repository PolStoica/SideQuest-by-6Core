import 'react-native-gesture-handler';
import React from 'react';

import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';

import Onboarding1 from './screens/onboarding1';
import Onboarding2 from './screens/onboarding2';
import Onboarding3 from './screens/onboarding3';
import Onboarding4 from './screens/onboarding4';

const Stack = createStackNavigator();

export default function App() {
    return (
        <NavigationContainer>
            <Stack.Navigator screenOptions={{ headerShown: false }}>
                <Stack.Screen name="onboarding1" component={Onboarding1} />
                <Stack.Screen name="onboarding2" component={Onboarding2} />
                <Stack.Screen name="onboarding3" component={Onboarding3} />
                <Stack.Screen name="onboarding4" component={Onboarding4} />
            </Stack.Navigator>
        </NavigationContainer>
    );
}