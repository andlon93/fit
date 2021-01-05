import * as React from 'react';
import { ActivityIndicator, StyleSheet, TouchableOpacity } from 'react-native';
import {
  NavigationParams,
  NavigationScreenProp,
  NavigationState,
} from 'react-navigation';
import { RouteProp } from '@react-navigation/native';
import { gql, useQuery, useMutation } from '@apollo/client'

import { Card, Header, ListItem, Button, Icon, Overlay } from 'react-native-elements';
import useColorScheme from '../hooks/useColorScheme';
import { Text, View } from '../components/Themed';
import { RootStackParamList, DetailsData, WorkoutDetails } from '../types';
import { secondsToDuration, secondsToWaterNeed, metersToString, dateToString } from './utility_functions';

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

const DELETE_WORKOUT = gql`
  mutation DeleteWorkout($id: String!) {
    deleteWorkout(deleteWorkoutRequest: { id: $id }) {
      id
    }
  }
`;

interface Props {
  navigation: NavigationScreenProp<NavigationState, NavigationParams>;
  route: RouteProp<RootStackParamList, 'WorkoutDetailScreen'>;
}

interface IconLabelContentPresenterProps {
  iconName: string;
  iconType?: string | undefined;
  iconColor?: string | undefined;
  label: string;
  content: string;
}

const IconLabelContentPresenter = (props : IconLabelContentPresenterProps) => {
  return (
    <ListItem style={styles.summaryItem}>        
      <Icon name={props.iconName} type={props.iconType} color={props.iconColor} />
      <ListItem.Content>
        <Text style={{ color: 'grey' }}>{props.label}</Text>
        <ListItem.Title>{props.content}</ListItem.Title>
      </ListItem.Content>
    </ListItem>
  );
};


export default function WorkoutDetailScreen(props : Props) {
  
  const colorScheme = useColorScheme();
  
  const { data, loading } = useQuery<DetailsData>(DETAILS_QUERY, {
    variables: { filter: {ids: [props.route.params?.id]} },
  });

  const [deleteWorkout] = useMutation<
    { deleteWorkout: WorkoutDetails },
    { id: string }
  >(DELETE_WORKOUT);

  const [isDialogVisible, setIsDialogVisible] = React.useState(false);

  const closeDialog = () => {
    setIsDialogVisible(false);
  }

  const showDeleteDialog = () => {
    setIsDialogVisible(true);
  }
  
  const handleDelete = async (): Promise<void> => {
    try {
      let response = await deleteWorkout({
        variables: {
          id: props.route.params?.id
        }
      });
    } catch (exception) {
      console.log(exception);
    }
  };

  if (loading || !data) {
    return <ActivityIndicator />
  }

  let workout = data.workouts[0];

  return (
    <View style={styles.container}>
      <Header
        barStyle={colorScheme == 'dark' ? 'dark-content' : 'light-content'}
        placement="left"    
        leftComponent={<Button
          icon={{ name: 'arrow-back', color: 'white' }}
          type="clear"
          onPress={() => props.navigation.goBack()}
        />}
        centerComponent={{ text: 'John Doe', color: 'white' }} // TODO: Use the name of the user
        rightComponent={<Button
          icon={
            <Icon
              name="delete"
              color="white"
            />
          }             
          onPress={showDeleteDialog}
        />}
        containerStyle={{
          justifyContent: 'space-around',
        }}

      />
      <Card>
        <ListItem>
          <Icon reverse name='directions-run' color='#517fa4'/>
          <ListItem.Content>
            <ListItem.Title>{workout.sport}</ListItem.Title>
            <Text style={{ color: 'grey' }}>{dateToString(new Date(workout.startTime))}</Text>
          </ListItem.Content>
        </ListItem>
      </Card>
      <Card>
        <View style={styles.summaryContainer}>
          <IconLabelContentPresenter 
            iconName='timer'
            iconColor='#3471eb'
            label='Varighet'
            content={secondsToDuration(workout.totalTimeSeconds)} />
            
          {workout.distance !== 0 && <IconLabelContentPresenter 
            iconName='place'
            iconColor='#8120f7'
            label='Distanse'
            content={metersToString(workout.distance, 2)} />}

          <IconLabelContentPresenter 
            iconName='speedometer-medium'
            iconType='material-community'
            iconColor='#5bb05e'
            label='Snittempo'
            content='X' />

          <IconLabelContentPresenter 
            iconName='speedometer'
            iconType='material-community'
            iconColor='#0a630d'
            label='Makstempo'
            content='X' />  
            
          <IconLabelContentPresenter 
            iconName='speedometer-medium'
            iconType='material-community'
            iconColor='#bf5050'
            label='Snittfart'
            content='X' />

          <IconLabelContentPresenter 
            iconName='speedometer'
            iconType='material-community'
            iconColor='#c20000'
            label='Maksfart'
            content='X' />

          {workout.calories && workout.calories !== null && <IconLabelContentPresenter 
            iconName='whatshot'
            iconColor='#f77a20'
            label='Kalorier'
            content={workout.calories.toString()} />}

          {workout.totalTimeSeconds && workout.totalTimeSeconds !== null && <IconLabelContentPresenter 
            iconName='water'
            iconType='entypo'
            iconColor='#3e9bc9'
            label='Væskebalanse'
            content={secondsToWaterNeed(workout.totalTimeSeconds)} />}

          {workout.cadence && workout.cadence !== 0 && <IconLabelContentPresenter 
              iconName='timer'
              iconColor='#9d3ec9'
              label='Tråkkfrekvens'
              content={workout.cadence.toString()} />}

          {workout.averageHeartRate && workout.averageHeartRate !== null && <IconLabelContentPresenter 
              iconName='favorite'
              iconColor='#bf5050'
              label='Gjennomsnittspuls'
              content={workout.averageHeartRate.toString()} />}

          {workout.maximumHeartRate && workout.maximumHeartRate !== null && <IconLabelContentPresenter 
              iconName='favorite'
              iconColor='#c20000'
              label='Makspuls'
              content={workout.maximumHeartRate.toString()} />}

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
            iconName='arrow-up-circle'
            iconType='feather'
            iconColor='#0b7478'
            label='Total stigning'
            content='X' />

          <IconLabelContentPresenter 
            iconName='arrow-down-circle'
            iconType='feather'
            iconColor='#6cc1c4'
            label='Total nedstigning'
            content='X' />
        </View>
      </Card>

      <Overlay overlayStyle={{width: '90%', backgroundColor: 'transparent', padding: 0}} isVisible={isDialogVisible} onBackdropPress={closeDialog}>
        <View darkColor='#424242' style={styles.dialog}>
          <Text style={styles.dialogMessage}>Vil du slette denne aktiviteten?</Text>
          <View darkColor='#424242' style={{flexDirection: 'row', alignSelf: 'flex-end'}}>            
            <TouchableOpacity style={styles.dialogBtn} onPress={closeDialog}>
              <Text style={styles.dialogBtnText}>Avbryt</Text>
            </TouchableOpacity>
            <View style={{width: 20}} darkColor='#424242'></View>
            <TouchableOpacity style={styles.dialogBtn} onPress={handleDelete}>
              <Text style={styles.dialogBtnText}>Slett</Text>
            </TouchableOpacity>
          </View>
        </View>
      </Overlay>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'stretch',
    justifyContent: 'flex-start',
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
  summaryContainer: {
    display: 'flex',
    flexDirection: 'row',
    flexWrap: 'wrap',
    width: '100%',
  },
  summaryItem: {
    width: '50%'
  },
  dialog: {
    padding: 20,
    borderRadius: 10,
  },
  dialogMessage: {
    fontSize: 16,
  },
  dialogBtn:{
    alignItems:"center",
    justifyContent:"center",
    marginTop:10,
  },
  dialogBtnText:{
    fontWeight: 'bold',
    color: '#7b9fed',
    fontSize: 16,
  },
});
