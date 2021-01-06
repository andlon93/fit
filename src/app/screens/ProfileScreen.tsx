import * as React from 'react';
import { StyleSheet } from 'react-native';
import { Card } from 'react-native-elements';

import { Text, View } from '../components/Themed';
import ProgressDiagram from '../components/ProgressDiagram';
import { minutesToDuration, percentageOfTimeSpan } from './utility_functions';

export default function ProfileScreen() {

  const workouts = 2;
  const workoutGoal = 100;

  const minutes = 120;
  const minutesGoal = 240;

  const today = new Date();
  const year = today.getFullYear();
  const month = today.getMonth();

  return (
    <Card>
      <Text style={styles.cardHeader}>Målt</Text>
      <View style={styles.container}>
        <ProgressDiagram
          percentage={(100.0 * workouts) / workoutGoal}
          expectedPercentage={percentageOfTimeSpan(new Date(year, 0, 1), new Date(year, 11, 31, 23, 59, 59))}
          textTop='I år'
          textCenter={workouts.toString()}
          textBottom={'av ' + workoutGoal + ' ganger'}
          color='green' />
        <ProgressDiagram
          percentage={(100.0 * minutes) / minutesGoal}
          expectedPercentage={percentageOfTimeSpan(new Date(year, month, 1), new Date(year, month + 1, 0, 23, 59, 59))}
          textTop='Denne måneden'
          textCenter={minutesToDuration(minutes)}
          textBottom={'av ' + minutesToDuration(minutesGoal)} />
      </View>
    </Card>
  );
}

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-around',
  },
  cardHeader: {
    fontWeight: 'bold',
    textTransform: 'uppercase',
    fontSize: 14,
    marginBottom: 5,
  },
});
