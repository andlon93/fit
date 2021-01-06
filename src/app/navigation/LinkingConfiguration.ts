import * as Linking from 'expo-linking';

export default {
  prefixes: [Linking.makeUrl('/')],
  config: {
    screens: {
      Root: {
        screens: {
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
          NewWorkoutScreen: {
            screens: {
              WorkoutScreen: 'newWorkout',
            },
          },
          ChallengesScreen: {
            screens: {
              ChallengesScreen: 'challenges',
            },
          },
          ProfileScreen: {
            screens: {
              ProfileScreen: 'profile',
            },
          },
          WorkoutDetailScreen: {
            screens: {
              WorkoutDetailScreen: 'workoutDetails',
            },
          },
          AddManualEntryScreen: {
            screens: {
              AddManualEntryScreen: 'addManualEntry',
            },
          },
        },
      },
      NotFound: '*',
    },
  },
};
