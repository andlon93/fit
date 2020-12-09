export type RootStackParamList = {
  Root: undefined;
  NotFound: undefined;
};

export type BottomTabParamList = {
  FeedTab: undefined;
  HistoryTab: undefined;
  WorkoutTab: undefined;
  ChallengesTab: undefined;
  ProfileTab: undefined;
};

export type FeedParamList = {
  FeedScreen: undefined;
};

export type HistoryParamList = {
  HistoryScreen: undefined;
};

export type WorkoutTabParamList = {
  WorkoutScreen: undefined;
};

export type ChallengesTabParamList = {
  ChallengesScreen: undefined;
};

export type ProfileTabParamList = {
  ProfileScreen: undefined;
};

export type HeaderParamList = {
  headerDisplay: string;
};

export type WorkoutListItem = {
  id: string;
  type: string;
  startTime: Date;
  durationSeconds: number;
};

export type Section = {
  key: SectionListItem;
  data: WorkoutListItem[];
}

export type SectionListItem = {
  header: string;
  number: number;
  distance: number;
  duration: number;
  calories: number;
}; 
