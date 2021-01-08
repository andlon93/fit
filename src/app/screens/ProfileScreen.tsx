import * as React from 'react';
import { StyleSheet } from 'react-native';
import { Card } from 'react-native-elements';

import { Text, View } from '../components/Themed';
import ProgressDiagram from '../components/ProgressDiagram';
import StatisticsDiagram from '../components/StatisticsDiagram';
import { minutesToDuration, percentageOfTimeSpan } from './utility_functions';

export default function ProfileScreen() {

  const workouts = 1;
  const workoutGoal = 100;

  const minutes = 30;
  const minutesGoal = 240;

  const today = new Date();
  const year = today.getFullYear();
  const month = today.getMonth();

  return (
    <View>
      <Card>
        <Text style={styles.cardHeader}>Mål</Text>
        <View style={styles.container}>
          <ProgressDiagram
            percentage={(100.0 * workouts) / workoutGoal}
            expectedPercentage={percentageOfTimeSpan(new Date(year, 0, 1), new Date(year, 11, 31, 23, 59, 59))}
            textTop='I år'
            textCenter={workouts.toString()}
            textBottom={'av ' + workoutGoal + ' ganger'} />
          <ProgressDiagram
            percentage={(100.0 * minutes) / minutesGoal}
            expectedPercentage={percentageOfTimeSpan(new Date(year, month, 1), new Date(year, month + 1, 0, 23, 59, 59))}
            textTop='Denne måneden'
            textCenter={minutesToDuration(minutes)}
            textBottom={'av ' + minutesToDuration(minutesGoal)} />
        </View>
      </Card>
      <Card>
        <Text style={styles.cardHeader}>Statistikk</Text>
        <View style={[styles.container]}>
          <StatisticsDiagram
            percentage={(100.0 * workouts) / workoutGoal} />
        </View>
      </Card>
    </View>
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
