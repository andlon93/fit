import React, { useEffect, useState } from 'react';
import { 
  ActivityIndicator,
  StyleSheet,
  TouchableWithoutFeedback, ScrollView, ListRenderItem } from 'react-native';
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
import { FlatList } from 'react-native-gesture-handler';

const FEED_QUERY = gql`
  query Feed($offset: Int, $rows: Int) {
    workouts(paging: { offset: $offset, rows: $rows }) {
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
  const pageSize = 10;
  const [offset, setOffset] = useState(0);

  useEffect(() => {
    if (offset === 0) return;
    console.info('NEXT PAGE');
    console.log('offset: ', offset);
    fetchMore({
      variables: {
        offset: offset,
        rows: pageSize,
      },
    });
  }, [offset]);

  const { loading, data, fetchMore } = useQuery<FeedData>(FEED_QUERY, {
    notifyOnNetworkStatusChange: true,
    variables: {
      offset: 0,
      rows: pageSize,
    },
  })

  const renderItem = ({ item }) => {
    let workout = item;
    return <TouchableWithoutFeedback onPress={() => props.navigation.navigate('WorkoutDetailScreen', { id: workout.id })}>
      <Card>
        <ListItem>
          <Icon reverse name='directions-run' color='#e69d17' />
          <ListItem.Content>
            <ListItem.Title>{dateToStringMinimal(new Date(workout.startTime))}</ListItem.Title>
            <Text style={{ color: 'grey' }}>{convertToSubtitle(workout)}</Text>
          </ListItem.Content>
        </ListItem>
      </Card>
    </TouchableWithoutFeedback>;
  } 

  return (
    <View>      
      {data && <FlatList
        data={data?.workouts}
        renderItem={renderItem}
        keyExtractor={(item, index) => item.id}
        onEndReachedThreshold={0.1}
        onEndReached={() => setOffset(offset => offset + pageSize)} />}
      {loading && <ActivityIndicator />}
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
});
