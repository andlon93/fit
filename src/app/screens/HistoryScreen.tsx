import React, { useEffect, useState } from 'react';
import { ActivityIndicator, FlatList, Image, SectionList, SectionListData, StyleSheet, TouchableWithoutFeedback } from 'react-native';
import { Ionicons } from '@expo/vector-icons';

import Colors from '../constants/Colors';
import useColorScheme from '../hooks/useColorScheme';
import { Text, View } from '../components/Themed';
import { Section, SectionListItem, WorkoutListItem } from '../types';

export default function HistoryScreen() {
  const colorScheme = useColorScheme();

  const [loading, setLoading] = useState(true);
  const [sections, setSections] = useState<Section[]>([]);

  useEffect(() => {
    var temp: Section[] = [
      {
        // key: 'November 2020',
        key: { header: 'November 2020', number: 8, distance: 33920, duration: 18300, calories: 2315 },
        data: [
          { id: '1', durationSeconds: 3600, startTime: new Date(2020, 18, 11, 19, 0, 0), type: "Løping" },
          { id: '2', durationSeconds: 3600, startTime: new Date(2020, 18, 11, 19, 0, 0), type: "Løping" },
          { id: '3', durationSeconds: 3600, startTime: new Date(2020, 18, 11, 19, 0, 0), type: "Løping" },
          { id: '4', durationSeconds: 3600, startTime: new Date(2020, 18, 11, 19, 0, 0), type: "Løping" },
          { id: '5', durationSeconds: 3600, startTime: new Date(2020, 18, 11, 19, 0, 0), type: "Løping" },
        ]
      },
      {
        key: { header: 'Oktober 2020', number: 8, distance: 33920, duration: 18300, calories: 2315 }, data: []
      },
      {
        key: { header: 'September 2020', number: 8, distance: 33920, duration: 18300, calories: 2315 }, data: []
      },
      {
        key: { header: 'August 2020', number: 8, distance: 33920, duration: 18300, calories: 2315 }, data: []
      },
    ];
    setSections(temp);
    setLoading(false);
  }, []);

  const sectionItem = (section: Section) => {
    return (
      <TouchableWithoutFeedback
        onPress={() => console.log('Navigate to workout: navigation.navigate(WorkoutDetail, {url: item.url}')}>
        <View style={styles.subheader}>
          <Text style={styles.title}>{section.key.number}</Text>
          <View>
            <Text style={styles.title}>{section.key.header}</Text>
            <View style={styles.subheaderItems}>
              <Text style={styles.subheaderItem}>{section.key.distance}</Text>
              <Text style={styles.subheaderItem}>{section.key.duration}</Text>
              <Text style={styles.subheaderItem}>{section.key.calories}</Text>
            </View>
          </View>
        </View>
      </TouchableWithoutFeedback>
    )
  };

  const workoutItem = (item: WorkoutListItem) => {
    return (
      <TouchableWithoutFeedback
        onPress={() => console.log('Navigate to workout: navigation.navigate(WorkoutDetail, {url: item.url}')}>
        <View style={styles.listings}>
          <Text style={styles.title}>{item.type}</Text>
          <Ionicons name="ios-walk" size={24} color={Colors[colorScheme].text} />
          <Text style={styles.duration}>{item.durationSeconds}</Text>
        </View>
      </TouchableWithoutFeedback>
    )
  };

  return (
    <View style={styles.container}>
      {loading ? <ActivityIndicator /> : (
        <SectionList
          sections={sections}
          renderItem={workoutItem}
          renderSectionHeader={({ section }) => sectionItem(section)}
          keyExtractor={(item: WorkoutListItem) => item.id}
        />
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'stretch',
    justifyContent: 'center',
    width: '100%',
    paddingLeft: 20,
  },
  title: {
    paddingBottom: 10,
    fontSize: 20,
    fontWeight: 'bold',
  },
  separator: {
    marginVertical: 30,
    height: 1,
    width: '80%',
  },
  header: {
    fontSize: 32,
    backgroundColor: '#fff',
  },
  listings: {
    paddingTop: 15,
    paddingBottom: 25,
  },
  subheader: {
    flex: 1,
    flexDirection: "row",
    alignItems: "center",
    paddingTop: 15,
    paddingBottom: 25,
  },
  subheaderItems: {
    flex: 1,
    flexDirection: "row",
  },
  subheaderItem: {
    fontSize: 14,
    paddingTop: 15,
    paddingLeft: 5,
    paddingBottom: 25,
  },
  duration: {

  },
});
