import React from 'react';
import { ActivityIndicator, SectionList, StyleSheet, TouchableWithoutFeedback, TouchableOpacity, SectionListData } from 'react-native';
import { ListItem, Icon } from 'react-native-elements';
import {
  NavigationParams,
  NavigationScreenProp,
  NavigationState,
} from 'react-navigation';
import { gql, useQuery } from '@apollo/client'

import useColorScheme from '../hooks/useColorScheme';
import { Text, View } from '../components/Themed';
import { Section, WorkoutListItem, SectionListItem, Workout, HistoryData, WorkoutGroup } from '../types';
import { AppLoading } from 'expo';
import { dateToStringMinimal, metersToString, secondsToDuration } from './utility_functions';

const HISTORY_QUERY = gql`
  query History {
    firstGroup: groupedWorkouts(paging: { rows: 1, offset: 0 }, groupBy: MONTH) {
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
    otherGroups: groupedWorkouts(paging: { rows: 4, offset: 1 }, groupBy: MONTH) {
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
interface IconWithTextProps {
  iconName: string;
  iconType?: string | undefined;
  iconColor?: string | undefined;
  content: string;
}

const IconWithText = (props : IconWithTextProps) => {
  return (
    <View style={styles.iconWithTextContainer}>
      <Icon name={props.iconName} color={props.iconColor} type={props.iconType} />
      <Text style={styles.subheaderItem}>{props.content}</Text>
    </View>
  );
};

interface Props {
  navigation: NavigationScreenProp<NavigationState, NavigationParams>;
}

export default function HistoryScreen(props : Props) {
  const colorScheme = useColorScheme();

  const { data, loading } = useQuery<HistoryData>(HISTORY_QUERY)

  if (loading) {
    return <AppLoading />
  }

  const sectionItem = (section: SectionListData<WorkoutListItem, SectionListItem>) => {
    return (
      <TouchableWithoutFeedback
        key={section.header}
        onPress={() => console.log('TODO: Expand/Collapse section: ' + section.header)}>
        <ListItem>
          <Text style={styles.numberOfWorkouts}>{section.number}</Text>
          <ListItem.Content>
            <Text style={styles.title}>{section.header}</Text>
            <ListItem.Subtitle>
              <IconWithText iconName='place' iconColor='#0384fc' content={metersToString(section.distance)} />
              <IconWithText iconName='timer' iconColor='#fc0303' content={secondsToDuration(section.duration)} />
              <IconWithText iconName='fire' iconColor='#fc9804' iconType='material-community' content={section.calories + ' kcal'} />
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
    var section: SectionListData<WorkoutListItem, SectionListItem> = {
      header: group.title, number: group.numberOfWorkouts, distance: group.distanceInMeters, duration: group.durationInSeconds, calories: group.calories,
      data: mapData ? group.workouts : []
    }
    return section;
  }

  const ConvertToSections = (data: HistoryData) => {
    var sections: SectionListData<WorkoutListItem, SectionListItem>[] = [ConvertToSection(data.firstGroup[0], true)]
    sections = sections.concat(data.otherGroups.map(group => ConvertToSection(group, false)))
    return sections;
  }

  return (
    <View style={styles.container}>
      {loading && data ? <ActivityIndicator /> : (
        <SectionList
          sections={ConvertToSections(data)}
          renderItem={workoutItem}
          renderSectionHeader={({ section }) => sectionItem(section)}
          keyExtractor={(item: WorkoutListItem) => item.id}
        />
      )}
      <TouchableOpacity
          activeOpacity={0.7}
          onPress={() => props.navigation.navigate('AddManualEntryScreen')}
          style={styles.touchableOpacityStyle}>
          <Icon name="add-circle" color="#BB3A87" size={50} />
        </TouchableOpacity>
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
  numberOfWorkouts: {
    paddingBottom: 10,
    fontSize: 20,
    fontWeight: 'bold',
  },
  title: {
    fontWeight: 'bold',
    textTransform: 'uppercase',
    fontSize: 14,
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
    flexDirection: 'row',
    alignItems: 'center',
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
  iconWithTextContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  touchableOpacityStyle: {
    position: 'absolute',
    width: 50,
    height: 50,
    alignItems: 'center',
    justifyContent: 'center',
    right: 10,
    bottom: 10,
  },
});
