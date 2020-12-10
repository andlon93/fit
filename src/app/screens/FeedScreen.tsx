import React from 'react';
import { ActivityIndicator, SectionList, StyleSheet, TouchableWithoutFeedback, ScrollView } from 'react-native';
import { Card, ListItem, Button, Icon, Avatar } from 'react-native-elements'
import { gql, useQuery } from '@apollo/client'

import useColorScheme from '../hooks/useColorScheme';
import { Text, View } from '../components/Themed';
import { Section, WorkoutListItem, Workout, HistoryData } from '../types';
import { AppLoading } from 'expo';

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

const convertDateToString = (date : Date) => {
  let temp2 = date.getDate() + '. ' + convertMonthToString(date.getMonth()) + (date.getFullYear() === new Date().getFullYear() ? '' : ' ' + date.getFullYear()) + ' kl. ' + date.getHours() + ':' + date.getMinutes();
  return temp2;
}

const convertMonthToString = (month : number) => {
  switch(month) { 
    case 1: { 
       return 'januar'; 
    }
    case 2: {
      return 'februar';
    }
    case 3: {
      return 'mars';
    }
    case 4: {
      return 'april';
    }
    case 5: {
      return 'mai';
    }
    case 6: {
      return 'juni';
    }
    case 7: {
      return 'juli';
    }
    case 8: {
      return 'august';
    }
    case 9: {
      return 'september';
    }
    case 10: {
      return 'oktober';
    }
    case 11: {
      return 'november';
    }
    case 12: {
      return 'desember';
    }
    default: {
       return '';
    } 
 }  
}

const convertToSubtitle = (workout : WorkoutListItem) => {
  
  return Math.round((workout.distance / 1000.0 + Number.EPSILON) * 100) / 100 + ' km'
    + ' ' + convertSecondsToDuration(workout.totalTimeSeconds) + ' ' + workout.calories + ' kcal';
}

const convertSecondsToDuration = (seconds : number) => {
  let hours = Math.floor(seconds / 60 / 60);
  seconds = seconds - hours * 60 * 60;
  let minutes = Math.floor((seconds - hours * 60 * 60) / 60);
  seconds = seconds - minutes * 60;
  return (hours == 0 ? '' : hours + ':') + minutes + ':' + seconds;
}

export default function FeedScreen() {
  const colorScheme = useColorScheme();

  const { data, loading } = useQuery<HistoryData>(FEED_QUERY)

  if (loading) {
    return <AppLoading />
  }

  return (
    <ScrollView>
      {
        data?.workouts.map((workout, i) => (
          <TouchableWithoutFeedback key={i}
            onPress={() => console.log('Navigate to: ' + workout.id)}>
            <Card>
              <ListItem>
                <Avatar rounded title='JD' />
                <ListItem.Content>
                  <ListItem.Title>{convertDateToString(new Date(workout.startTime))}</ListItem.Title>
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
