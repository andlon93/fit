import React from 'react';
import { StyleSheet, ScrollView } from 'react-native';
import { Card, ListItem, Icon } from 'react-native-elements';
import {
  NavigationParams,
  NavigationScreenProp,
  NavigationState,
} from 'react-navigation';
import { gql, useQuery } from '@apollo/client'

import { Text } from '../components/Themed';
import useColorScheme from '../hooks/useColorScheme';
import { ChallengesData } from '../types';
import { AppLoading } from 'expo';
import { challengeScoreToString, daysLeftUntilAsString, minutesToDuration } from './utility_functions';

const CHALLENGE_QUERY = gql`
  query Challenges {
    challenges {
      name
      type
      startTime
      endTime
      leaderboard {
        user {
          id
          firstName
        }
        rank
        score
      }
    }
  }
  
`

interface Props {
  navigation: NavigationScreenProp<NavigationState, NavigationParams>;
}

export default function ChallengesScreen(props : Props) {
  const colorScheme = useColorScheme();

  const { data, loading } = useQuery<ChallengesData>(CHALLENGE_QUERY)

  if (loading) {
    return <AppLoading />
  }

  return (
    <ScrollView>
      {
        data?.challenges.map((challenge, i) => (
          <Card key={challenge.name}>
            <Text style={styles.daysLeftTag}>{daysLeftUntilAsString(new Date(challenge.endTime))}</Text>
            <Text style={styles.cardHeader}>{challenge.name}</Text>
            {challenge.leaderboard.map((participant, i) => (
              <ListItem key={participant.user.id}>
                <Icon reverse size={20} name='directions-run' color='#076263' />
                <ListItem.Content>
                  <ListItem.Title>{participant.rank + '. ' + participant.user.firstName}</ListItem.Title>
                  <ListItem.Subtitle>{challengeScoreToString(challenge.type, participant.score)}</ListItem.Subtitle>
                </ListItem.Content>
              </ListItem>
            ))}
          </Card>
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
  },
  cardHeader: {
    fontWeight: 'bold',
    textTransform: 'uppercase',
    fontSize: 14,
  },
  daysLeftTag: {
    fontWeight: 'bold',
    textTransform: 'uppercase',
    backgroundColor: '#076263',
    color: '#fff',
    paddingTop: 2,
    paddingBottom: 2,
    paddingLeft: 5,
    paddingRight: 5,
    alignSelf: 'flex-start',
    fontSize: 12,
  }
});
