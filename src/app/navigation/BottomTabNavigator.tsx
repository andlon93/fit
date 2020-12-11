import { Icon } from 'react-native-elements'
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import * as React from 'react';

import Colors from '../constants/Colors';
import useColorScheme from '../hooks/useColorScheme';
import FeedScreen from '../screens/FeedScreen';
import HistoryScreen from '../screens/HistoryScreen';
import WorkoutScreen from '../screens/WorkoutScreen';
import ChallengesScreen from '../screens/ChallengesScreen';
import ProfileScreen from '../screens/ProfileScreen';
import { BottomTabParamList, FeedParamList, HistoryParamList, WorkoutTabParamList, ChallengesTabParamList, ProfileTabParamList } from '../types';

const BottomTab = createBottomTabNavigator<BottomTabParamList>();
const FeedTabName = "Nyhetsstrøm";
const HistoryTabName = "Historikk";
const WorkoutTabName = "Treningsøkt";
const ChallengesTabName = "Utfordringer";
const ProfileTabName = "Profile";

export default function BottomTabNavigator() {
  const colorScheme = useColorScheme();

  return (
    <BottomTab.Navigator
      initialRouteName="FeedTab"
      tabBarOptions={{ activeTintColor: Colors[colorScheme].tint }}>
      <BottomTab.Screen
        name="FeedTab"
        component={FeedTabNavigator}
        options={{
          tabBarIcon: ({ color }) => <TabBarIcon name="people" color={color} />,
        }}
      />
      <BottomTab.Screen
        name="HistoryTab"
        component={HistoryTabNavigator}
        options={{
          tabBarIcon: ({ color }) => <TabBarIcon name="profile" type='antdesign' color={color} />,
        }}
      />
      <BottomTab.Screen
        name="WorkoutTab"
        component={WorkoutTabNavigator}
        options={{
          tabBarIcon: ({ color }) => <TabBarIcon name="dashboard" type='antdesign' color={color} />,
        }}
      />
      <BottomTab.Screen
        name="ChallengesTab"
        component={ChallengesTabNavigator}
        options={{
          tabBarIcon: ({ color }) => <TabBarIcon name="trophy" type='simple-line-icon' color={color} />,
        }}
      />
      <BottomTab.Screen
        name="ProfileTab"
        component={ProfileTabNavigator}
        options={{
          tabBarIcon: ({ color }) => <TabBarIcon name="person" color={color} />,
        }}
      />
    </BottomTab.Navigator>
  );
}

// You can explore the built-in icon families and icons on the web at:
// https://icons.expo.fyi/
function TabBarIcon(props: { name: string; type?: string, color: string }) {
  return <Icon size={30} style={{ marginBottom: -3 }} {...props} />;
}

// Each tab has its own navigation stack, you can read more about this pattern here:
// https://reactnavigation.org/docs/tab-based-navigation#a-stack-navigator-for-each-tab
const FeedTabStack = createStackNavigator<FeedParamList>();

function FeedTabNavigator() {
  return (
    <FeedTabStack.Navigator>
      <FeedTabStack.Screen
        name="FeedScreen"
        component={FeedScreen}
        options={{ headerTitle: FeedTabName }}
      />
    </FeedTabStack.Navigator>
  );
}

const HistoryTabStack = createStackNavigator<HistoryParamList>();

function HistoryTabNavigator() {
  return (
    <HistoryTabStack.Navigator>
      <HistoryTabStack.Screen
        name="HistoryScreen"
        component={HistoryScreen}
        options={{ headerTitle: HistoryTabName }}
      />
    </HistoryTabStack.Navigator>
  );
}

const WorkoutTabStack = createStackNavigator<WorkoutTabParamList>();

function WorkoutTabNavigator() {
  return (
    <WorkoutTabStack.Navigator>
      <WorkoutTabStack.Screen
        name="WorkoutScreen"
        component={WorkoutScreen}
        options={{ headerTitle: WorkoutTabName }}
      />
    </WorkoutTabStack.Navigator>
  );
}

const ChallengesTabStack = createStackNavigator<ChallengesTabParamList>();

function ChallengesTabNavigator() {
  return (
    <ChallengesTabStack.Navigator>
      <ChallengesTabStack.Screen
        name="ChallengesScreen"
        component={ChallengesScreen}
        options={{ headerTitle: ChallengesTabName }}
      />
    </ChallengesTabStack.Navigator>
  );
}

const ProfileTabStack = createStackNavigator<ProfileTabParamList>();

function ProfileTabNavigator() {
  return (
    <ProfileTabStack.Navigator>
      <ProfileTabStack.Screen
        name="ProfileScreen"
        component={ProfileScreen}
        options={{ headerTitle: ProfileTabName }}
      />
    </ProfileTabStack.Navigator>
  );
}