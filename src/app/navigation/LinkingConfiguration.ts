import * as Linking from 'expo-linking';

export default {
  prefixes: [Linking.makeUrl('/')],
  config: {
    screens: {
      Root: {
        screens: {
          ChallengesScreen: {
            screens: {
              ChallengesScreen: 'challenges',
            },
          },
          FeedScreen: {
            screens: {
              FeedScreen: 'feed',
            },
          },
          HistoryScreen: {
            screens: {
              HistoryScreen: 'history',
            },
          },
          ProfileScreen: {
            screens: {
              ProfileScreen: 'profile',
            },
          },
          WorkoutScreen: {
            screens: {
              WorkoutScreen: 'workoutDetails',
            },
          },
        },
      },
      NotFound: '*',
    },
  },
};
