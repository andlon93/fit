export type RootStackParamList = {
  Root: undefined;
  NotFound: undefined;
  WorkoutDetailScreen: { id: string } | undefined;
  AddManualEntryScreen: undefined;
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
  firstGroup: WorkoutGroup[];
  otherGroups: WorkoutGroup[];
}


export interface FeedData {
  workouts: WorkoutListItem[];
}

export type WorkoutGroup = {
  title: string;
  numberOfWorkouts: number;
  durationInSeconds: number;
  distanceInMeters: number;
  calories: number;
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
  positions: Position[];
};

export interface Position {
  heartRate: number;
  time: Date;
}

export enum Sport {
  AEROBICS,
  BACKCOUNTRY_SKIING,
  BADMINTON,
  BASEBALL,
  BASKETBALL,
  BOXING,
  CANICROSS,
  CIRCUIT_TRAINING,
  CLIMBING,
  CLIMBING_STAIRS,
  CRICKET,
  CROSS_COUNTRY_SKIING,
  CROSSFIT,
  CYCLING_HAND,
  CYCLING_SPORT,
  CYCLING_TRANSPORT,
  DANCING,
  ELLIPTICAL,
  FENCING,
  FITNESS_WALKING,
  FLOORBALL,
  FOOTBALL_AMERICAN,
  FOOTBALL_RUGBY,
  FOOTBALL_SOCCER,
  GOLFING,
  GYMNASTICS,
  HANDBALL,
  HIKING,
  HOCKEY,
  ICE_SKATING,
  INDOOR_CYCLING,
  KAYAKING,
  KICK_SCOOTER,
  KITE_SURFING,
  MARTIAL_ARTS,
  MOUNTAIN_BIKING,
  ORIENTEERING,
  OTHER,
  PADDLE_TENNIS,
  PARAGLIDING,
  PILATES,
  POLO,
  RIDING,
  ROLLER_SKATING,
  ROLLER_SKIING,
  ROPE_JUMPING,
  ROWING,
  ROWING_INDOOR,
  RUNNING,
  SAILING,
  SCUBA_DIVING,
  SKATEBOARDING,
  SKI_TOURING,
  SKIING_DOWNHILL,
  SNOWBOARDING,
  SNOWSHOEING,
  SQUASH,
  STAND_UP_PADDLING,
  STRETCHING,
  SURFING,
  SWIMMING,
  TABLE_TENNIS,
  TENNIS,
  TRAIL_RUNNING,
  TREADMILL_WALKING,
  TREADMILL_RUNNING,
  VOLLEYBALL_BEACH,
  VOLLEYBALL_INDOOR,
  WALKING,
  WEIGHT_TRAINING,
  WHEELCHAIR,
  WINDSURFING,
  YOGA,
}

export interface WorkoutInput {
  sport: string;
  startTime: Date;
  totalTimeSeconds: number;
}

// Challenges
export interface User {
  id: string;
  firstName: string;
}

export interface Leaderboard {
  user: User;
  rank: number;
  score: number;
}

export enum ChallengeType {
  MOST_WORKOUTS,
  MOST_ACTIVE_MINUTES,
}

export interface Challenge {
  name: string;
  type: ChallengeType;
  startTime: Date;
  endTime: Date;
  leaderboard: Leaderboard[];
}

export interface ChallengesData {
  challenges: Challenge[];
}