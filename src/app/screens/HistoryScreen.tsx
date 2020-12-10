import React from 'react';
import { ActivityIndicator, SectionList, StyleSheet, TouchableWithoutFeedback } from 'react-native';
import { ListItem, Icon } from 'react-native-elements';
import {
  NavigationParams,
  NavigationScreenProp,
  NavigationState,
} from 'react-navigation';
import { gql, useQuery } from '@apollo/client'

import useColorScheme from '../hooks/useColorScheme';
import { Text, View } from '../components/Themed';
import { Section, WorkoutListItem, Workout, HistoryData, WorkoutGroup } from '../types';
import { AppLoading } from 'expo';
import { dateToStringMinimal, metersToString, secondsToDuration } from './utility_functions';

const HISTORY_QUERY = gql`
  query History {
    firstGroup: groupedWorkouts(paging:{rows:1 offset:0} groupBy:MONTH ) {
      title
      numberOfWorkouts
      durationInSeconds
      distanceInMeters
      calories
      workouts {
        id
        sport
        startTime
        calories
        totalTimeSeconds
        distance
      }
    }
    otherGroups: groupedWorkouts(paging:{rows:4 offset:1} groupBy:MONTH ) {
      title
      numberOfWorkouts
      durationInSeconds
      distanceInMeters
      calories
      workouts {
        id
      }
    }
  }
`

interface Props {
  navigation: NavigationScreenProp<NavigationState, NavigationParams>;
}

export default function HistoryScreen(props : Props) {
  const colorScheme = useColorScheme();

  const { data, loading } = useQuery<HistoryData>(HISTORY_QUERY)

  if (loading) {
    return <AppLoading />
  }

  const sectionItem = (section: Section) => {
    return (
      <TouchableWithoutFeedback
        key={section.key.header}
        onPress={() => console.log('TODO: Expand/Collapse section: ' + section.key.header)}>
        <ListItem>
          <Text style={styles.title}>{section.key.number}</Text>
          <ListItem.Content>
            <ListItem.Title>{section.key.header}</ListItem.Title>
            <ListItem.Subtitle>
              <Icon name='place' color='#0384fc' />
              <Text style={styles.subheaderItem}>{metersToString(section.key.distance)}</Text>
              <Icon name='timer' color='#fc0303' />
              <Text style={styles.subheaderItem}>{secondsToDuration(section.key.duration)}</Text>
              <Icon name='fire' color='#fc9804' type='material-community' />
              <Text style={styles.subheaderItem}>{section.key.calories + ' kcal'}</Text>
            </ListItem.Subtitle>
          </ListItem.Content>
        </ListItem>
      </TouchableWithoutFeedback>
    )
  };

  const convertToSubtitle = (workout : WorkoutListItem) => {  
    return metersToString(workout.distance, 2)
      + ' ' + secondsToDuration(workout.totalTimeSeconds, false) + ' ' + workout.calories + ' kcal';
  }

  const workoutItem = (workout: Workout) => {
    return (
      <TouchableWithoutFeedback
        key={workout.item.id}
        onPress={() => props.navigation.navigate('WorkoutDetailScreen', { id: workout.item.id })}>
        <ListItem key={workout.item.id}>
          <Icon name='rowing' color='#03cefc' reverse />
          <ListItem.Content>
            <ListItem.Title>{dateToStringMinimal(new Date(workout.item.startTime))}</ListItem.Title>
            <ListItem.Subtitle>{convertToSubtitle(workout.item)}</ListItem.Subtitle>
          </ListItem.Content>
        </ListItem>
      </TouchableWithoutFeedback>
    )
  };

  const ConvertToSection = (group: WorkoutGroup, mapData: boolean) => {
    var section: Section = {
      key: { header: group.title, number: group.numberOfWorkouts, distance: group.distanceInMeters, duration: group.durationInSeconds, calories: group.calories },
      data: mapData ? group.workouts : []
    }
    return section;
  }

  const ConvertToSections = (data: HistoryData) => {
    var sections: Section[] = [ConvertToSection(data.firstGroup[0], true)]
    sections = sections.concat(data.otherGroups.map(group => ConvertToSection(group, false)))
    return sections;
  }

  return (
    <View style={styles.container}>
      {loading ? <ActivityIndicator /> : (
        <SectionList
          sections={ConvertToSections(data)}
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
