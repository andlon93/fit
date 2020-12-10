import * as React from 'react';
import { StyleSheet } from 'react-native';
import {
  NavigationParams,
  NavigationScreenProp,
  NavigationState,
} from 'react-navigation';
import { RouteProp } from '@react-navigation/native';
import { gql, useQuery } from '@apollo/client'

import EditScreenInfo from '../components/EditScreenInfo';
import { Card, ListItem, Button, Icon, Avatar } from 'react-native-elements';
import { Text, View } from '../components/Themed';
import { RootStackParamList, DetailsData } from '../types';
import { AppLoading } from 'expo';
import { secondsToDuration, metersToString, dateToString } from './utility_functions';

const DETAILS_QUERY = gql`
  query Details($filter: WorkoutFilter) {
    workouts(filter: $filter) {
      id
      startTime
      totalTimeSeconds
      calories
      sport
      distance
      averageHeartRate
      maximumHeartRate
      cadence
    }
  }
  
`

interface Props {
  navigation: NavigationScreenProp<NavigationState, NavigationParams>;
  route: RouteProp<RootStackParamList, 'WorkoutDetailScreen'>;
}

interface IconLabelContentPresenterProps {
  iconName: string;
  iconColor?: string | undefined;
  label: string;
  content: string;
}

const IconLabelContentPresenter = (props : IconLabelContentPresenterProps) => {
  return (
    <ListItem leftIcon={{name: props.iconName, color: props.iconColor}}>
      <ListItem.Content>
      <ListItem.Subtitle>{props.label}</ListItem.Subtitle>
        <ListItem.Title>{props.content}</ListItem.Title>
      </ListItem.Content>
    </ListItem>
  );
};


export default function WorkoutDetailScreen(props : Props) {
  
  const { data, loading } = useQuery<DetailsData>(DETAILS_QUERY, {
    variables: { filter: {ids: [props.route.params?.id]} },
  });

  if (loading || !data) {
    return <AppLoading />
  }

  let workout = data.workouts[0];

  return (
    <View style={styles.container}>
      <Card>
        <ListItem>
          <Icon reverse name='sport' color='#0384fc'/>
          <ListItem.Content>
            <ListItem.Title>{workout.sport}</ListItem.Title>
            <ListItem.Subtitle>{dateToString(new Date(workout.startTime))}</ListItem.Subtitle>
          </ListItem.Content>
        </ListItem>
      </Card>
      <Card>
        <View style={styles.itemsContainer}>
          <IconLabelContentPresenter 
            iconName='timer'
            iconColor='#3471eb'
            label='Varighet'
            content={secondsToDuration(workout.totalTimeSeconds)} />
            
          <IconLabelContentPresenter 
            iconName='place'
            iconColor='#8120f7'
            label='Distanse'
            content={metersToString(workout.distance, 2)} />

          <IconLabelContentPresenter 
            iconName='speed'
            iconColor='#5bb05e'
            label='Snittempo'
            content='X' />

          <IconLabelContentPresenter 
              iconName='speed'
              iconColor='#0a630d'
              label='Makstempo'
              content='X' />  
            
          <IconLabelContentPresenter 
            iconName='speed'
            iconColor='#bf5050'
            label='Snittfart'
            content='X' />

          <IconLabelContentPresenter 
              iconName='speed'
              iconColor='#c20000'
              label='Maksfart'
              content='X' />

          <IconLabelContentPresenter 
              iconName='whatshot'
              iconColor='#f77a20'
              label='Kalorier'
              content={workout.calories.toString()} />

          <IconLabelContentPresenter 
            iconName='invert_colors'
            iconColor='#3e9bc9'
            label='Væskebalanse'
            content='X' />

          <IconLabelContentPresenter 
              iconName='timer'
              iconColor='#9d3ec9'
              label='Tråkkfrekvens'
              content={workout.cadence.toString()} />

          <IconLabelContentPresenter 
              iconName='favorite'
              iconColor='#bf5050'
              label='Gjennomsnittspuls'
              content={workout.averageHeartRate.toString()} />

          <IconLabelContentPresenter 
              iconName='favorite'
              iconColor='#c20000'
              label='Makspuls'
              content={workout.maximumHeartRate.toString()} />

          <IconLabelContentPresenter 
            iconName='terrain'
            iconColor='#4c83a6'
            label='Min. høyde'
            content='X' />

          <IconLabelContentPresenter 
            iconName='terrain'
            iconColor='#054d7a'
            label='Maks høyde'
            content='X' />
          
          <IconLabelContentPresenter 
            iconName='arrow_circle_up'
            iconColor='#0b7478'
            label='Total stigning'
            content='X' />

          <IconLabelContentPresenter 
            iconName='arrow_circle_down'
            iconColor='#6cc1c4'
            label='Total nedstigning'
            content='X' />
        </View>
      </Card>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  title: {
    fontSize: 20,
    fontWeight: 'bold',
  },
  separator: {
    marginVertical: 30,
    height: 1,
    width: '80%',
  },
  itemsContainer: {
    display: 'flex',
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
  },
});
