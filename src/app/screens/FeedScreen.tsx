import React from 'react';
import { 
  ActivityIndicator,
  StyleSheet,
  TouchableWithoutFeedback, ScrollView } from 'react-native';
import { Card, ListItem, Icon } from 'react-native-elements';
import {
  NavigationParams,
  NavigationScreenProp,
  NavigationState,
} from 'react-navigation';
import { gql, useQuery } from '@apollo/client'

import useColorScheme from '../hooks/useColorScheme';
import { Text, View } from '../components/Themed';
import { WorkoutListItem, FeedData } from '../types';
import { secondsToDuration, metersToString, dateToStringMinimal } from './utility_functions';

const FEED_QUERY = gql`
  query Feed {
    workouts(paging: {rows: 10}) {
      id
      startTime
      totalTimeSeconds
      calories
      sport
      distance
    }
  }
`

const convertToSubtitle = (workout : WorkoutListItem) => {
  
  return metersToString(workout.distance, 2)
    + ' ' + secondsToDuration(workout.totalTimeSeconds, false) + ' ' + workout.calories + ' kcal';
}

interface Props {
  navigation: NavigationScreenProp<NavigationState, NavigationParams>;
}

export default function FeedScreen(props : Props) {
  const colorScheme = useColorScheme();

  const { data, loading } = useQuery<FeedData>(FEED_QUERY)

  if (loading) {
    return <ActivityIndicator />
  }

  return (
    <ScrollView>
      {
        data?.workouts.map((workout, i) => (
          <TouchableWithoutFeedback key={i}
            onPress={() => props.navigation.navigate('WorkoutDetailScreen', { id: workout.id })}>
            <Card>
              <ListItem>
                <Icon reverse name='directions-run' color='#517fa4' />
                <ListItem.Content>
                  <ListItem.Title>{dateToStringMinimal(new Date(workout.startTime))}</ListItem.Title>
                  <ListItem.Subtitle>{convertToSubtitle(workout)}</ListItem.Subtitle>
                </ListItem.Content>
              </ListItem>
            </Card>
          </TouchableWithoutFeedback>
        ))
      }
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'stretch',
    justifyContent: 'center',
    width: '100%',
    paddingLeft: 20,
  }
});
