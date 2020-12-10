export type RootStackParamList = {
  Root: undefined;
  NotFound: undefined;
  WorkoutDetailScreen: { id : string } | undefined;
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
  sport: string;
  startTime: Date;
  totalTimeSeconds: number;  
  calories: number;
  distance: number;
};

export type Section = {
  key: SectionListItem;
  data: WorkoutListItem[];
}


export type Workout = {
  section: SectionListItem;
  item: WorkoutListItem;
}

export type SectionListItem = {
  header: string;
  number: number;
  distance: number;
  duration: number;
  calories: number;
}; 

export interface HistoryData {
  workouts: WorkoutListItem[];
}

export interface DetailsData {
  workouts: WorkoutDetails[];
}

export type WorkoutDetails = {
  id: string;
  startTime: Date;
  totalTimeSeconds: number;
  calories: number;
  sport: string;
  distance: number;
  averageHeartRate: number;
  maximumHeartRate: number;
  cadence: number;
};

export enum Sport {
  Biking = "Biking",
  Other = "Other",
  Running = "Running",
}