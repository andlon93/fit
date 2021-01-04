import * as React from 'react';
import { GestureResponderEvent, StyleSheet, Platform, TouchableOpacity } from 'react-native';
import {
  NavigationParams,
  NavigationScreenProp,
  NavigationState,
} from 'react-navigation';
import { RouteProp } from '@react-navigation/native';
import { gql, useMutation } from '@apollo/client'

import { Card, Header, ListItem, Button, Icon, Avatar, Input, Overlay } from 'react-native-elements';
import useColorScheme from '../hooks/useColorScheme';
import { Text, View } from '../components/Themed';
import { RootStackParamList, Sport, WorkoutDetails, WorkoutInput } from '../types';
import { AppLoading } from 'expo';
import { secondsToDuration, metersToString, dateToString } from './utility_functions';
// import DateTimePicker from '@react-native-community/datetimepicker';

interface IconLabelContentPresenterProps {
  iconName: string;
  iconType?: string | undefined;
  iconColor?: string | undefined;
  label?: string;
  content?: string;
  onPress?: (event : GestureResponderEvent) => void | undefined;
}

const IconLabelContentPresenter = (props : IconLabelContentPresenterProps) => {
  return (    
    <TouchableOpacity onPress={props.onPress}>
      <ListItem>
        <Icon name={props.iconName} type={props.iconType} color={props.iconColor} />
        <ListItem.Content>
        <ListItem.Title>{props.label}</ListItem.Title>
        </ListItem.Content>
        <Text>{props.content}</Text>
      </ListItem>
  </TouchableOpacity>
  );
};

const ADD_WORKOUT = gql`
  mutation AddWorkout($workoutInput: WorkoutInput!) {
    createWorkout(createWorkoutRequest: $workoutInput) {
      id
    }
  }
`;

interface Props {
  navigation: NavigationScreenProp<NavigationState, NavigationParams>;
  route: RouteProp<RootStackParamList, 'AddManualEntryScreen'>;
}

export default function AddManualEntryScreen(props : Props) {
  
  const colorScheme = useColorScheme();
  
  const [sport, setSport] = React.useState(Sport[Sport.RUNNING]);
  const [hours, setHours] = React.useState('0');
  const [minutes, setMinutes] = React.useState('30');
  const [seconds, setSeconds] = React.useState('0');
  const [startTime, setStartTime] = React.useState(new Date());
  const [distance, setDistance] = React.useState('0');
  const [distanceDecimal, setDistanceDecimal] = React.useState('0');

  const [addWorkout] = useMutation<
    { addWorkout: WorkoutDetails },
    { workoutInput: WorkoutInput }
  >(ADD_WORKOUT);

  const [visibleSportPicker, setVisibleSportPicker] = React.useState(false);
  const [visibleDurationPicker, setVisibleDurationPicker] = React.useState(false);
  const [visibleStartTimePicker, setVisibleStartTimePicker] = React.useState(false);
  const [visibleDistancePicker, setVisibleDistancePicker] = React.useState(false);

  const toggleSportPicker = () => {    
    console.log('Show sport picker');
    setVisibleSportPicker(!visibleSportPicker);
  };

  const toggleDurationPicker = () => {    
    console.log('Show duration picker');
    setVisibleDurationPicker(!visibleDurationPicker);
  };

  const toggleStartTimePicker = () => {    
    console.log('Show start time picker');
    setVisibleStartTimePicker(!visibleStartTimePicker);
  };

  const toggleDistancePicker = () => {    
    console.log('Show distance picker');
    setVisibleDistancePicker(!visibleDistancePicker);
  };

  const getTotalTimeSeconds = () => {
    return parseInt(hours, 10) * 3600 + parseInt(minutes, 10) * 60 + parseInt(seconds, 10)
  };

  const getDistanceMeters = () => {
    let decimal = parseInt(distanceDecimal, 10);
    return parseInt(distance, 10) * 1000 + decimal * 100 / Math.pow(10, Math.floor(Math.log(decimal)));
  };

  const onChange = (event : any, selectedDate : any) => {
    const currentDate = selectedDate || startTime;
    setVisibleStartTimePicker(Platform.OS === 'ios');
    setStartTime(currentDate);
  };

  const handleSave = async (): Promise<void> => {
    try {

      let response = await addWorkout({
        variables: {
          workoutInput:
          {
            startTime: startTime,
            sport: sport,
            totalTimeSeconds: getTotalTimeSeconds(),
          }
        }
      });

      // Navigate to newly created recipe.
      if (response?.data?.addWorkout?.id) {
        props.navigation.navigate('WorkoutDetailScreen', { id: response.data.addWorkout.id })
      }
    } catch (exception) {
      console.log(exception);
    }
  };

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
        centerComponent={{ text: 'Legg til aktivitet', style: { color: '#fff' } }}
        rightComponent={<Button
          title='Lagre'       
          onPress={handleSave}
        />}
      />
      <IconLabelContentPresenter onPress={toggleSportPicker} iconName='home' label='Aktivitet' content={sport} />

      <IconLabelContentPresenter onPress={toggleDurationPicker} iconName='timer' label='Varighet' content={secondsToDuration(getTotalTimeSeconds())} />
      
      <IconLabelContentPresenter onPress={toggleStartTimePicker} iconName='calendar' iconType='antdesign' label='Starttid' content={dateToString(startTime)} />

      <IconLabelContentPresenter onPress={toggleDistancePicker} iconName='map-marker-distance' iconType='material-community' label='Lengde' content={metersToString(getDistanceMeters(), 2)} />

      <Overlay isVisible={visibleSportPicker} onBackdropPress={toggleSportPicker}>
        <View>
          <Text>Velg aktivitet</Text>
          {
            Object.keys(Sport)
            .filter((value) => isNaN(Number(value)) === true)
            .map((key, i) => (
              <IconLabelContentPresenter onPress={() => setSport(key)} iconName='home' iconColor='#CC5C70' label={key} />
            ))
          }
        </View>
      </Overlay>

      <Overlay isVisible={visibleDurationPicker} onBackdropPress={toggleDurationPicker}>
        <View>
          <Input
            keyboardType='numeric'
            style={{ height: 40, borderColor: 'gray', borderWidth: 1 }}
            onChangeText={value => setHours(value.replace(/\D/g,''))}
            value={hours}
          />
          <Input
            keyboardType='numeric'
            style={{ height: 40, borderColor: 'gray', borderWidth: 1 }}
            onChangeText={value => setMinutes(value.replace(/\D/g,''))}
            value={minutes}
          />
          <Input
            keyboardType='numeric'
            style={{ height: 40, borderColor: 'gray', borderWidth: 1 }}
            onChangeText={value => setSeconds(value.replace(/\D/g,''))}
            value={seconds}
          />
        </View>
      </Overlay>

      <Overlay isVisible={visibleStartTimePicker} onBackdropPress={toggleStartTimePicker}>
        <View>          
          <Input
            style={{ height: 40, borderColor: 'gray', borderWidth: 1 }}
            onChangeText={text => setStartTime(new Date())}
            value={startTime.toDateString()}
          />
        </View>
      </Overlay>

      {/* {visibleStartTimePicker && (
        <DateTimePicker
          testID="dateTimePicker"
          value={startTime}
          mode='date'
          is24Hour={true}
          display="default"
          onChange={onChange}
        />
      )} */}

      <Overlay isVisible={visibleDistancePicker} onBackdropPress={toggleDistancePicker}>
        <View>
          <Input
            keyboardType='numeric'
            style={{ height: 40, borderColor: 'gray', borderWidth: 1 }}
            onChangeText={value => setDistance(value.replace(/\D/g,''))}
            value={distance}
          />
          <Input
            keyboardType='numeric'
            style={{ height: 40, borderColor: 'gray', borderWidth: 1 }}
            onChangeText={value => setDistanceDecimal(value.replace(/\D/g,''))}
            value={distanceDecimal}
          />
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
});
