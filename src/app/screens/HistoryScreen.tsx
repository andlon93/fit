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
    <View style={{
      flexDirection: 'row',
      alignItems: 'center',
    }}>
      <Icon size={16} name={props.iconName} color={props.iconColor} type={props.iconType} />
      <View style={{width: 2}} />
      <Text>{props.content}</Text>
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
    return <ActivityIndicator />
  }

  const sectionItem = (section: SectionListData<WorkoutListItem, SectionListItem>) => {
    return (
      <TouchableWithoutFeedback
        key={section.header}
        onPress={() => console.log('TODO: Expand/Collapse section: ' + section.header)}>
        <ListItem>
          <Text style={styles.numberOfWorkouts}>{section.number}</Text>
          <View>
            <Text style={styles.title}>{section.header}</Text>
            <View style={{flexDirection: 'row'}}>
              <IconWithText iconName='place' iconColor='#783ba1' content={metersToString(section.distance)} />
              <View style={{width: 5}} />
              <IconWithText iconName='timer' iconColor='#3285a8' content={secondsToDuration(section.duration)} />
              <View style={{width: 5}} />
              <IconWithText iconName='fire' iconColor='#fc9804' iconType='material-community' content={section.calories + ' kcal'} />
            </View>
          </View>
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
          <Icon name='rowing' color='#e69d17' reverse />
          <ListItem.Content>
            <ListItem.Title>{dateToStringMinimal(new Date(workout.item.startTime))}</ListItem.Title>
            <Text style={{ color: 'grey' }}>{convertToSubtitle(workout.item)}</Text>
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
          <Icon name="add-circle" color="#e69d17" size={50} />
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
