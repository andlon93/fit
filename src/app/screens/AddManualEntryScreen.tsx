import * as React from 'react';
import { 
  GestureResponderEvent,
  StyleSheet,
  Platform,
  TouchableOpacity,
  FlatList } from 'react-native';
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
import { secondsToDuration, metersToString, dateToString } from './utility_functions';
import DateTimePicker from '@react-native-community/datetimepicker';

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
  const [startTimePickerMode, setStartTimePickerMode] = React.useState<'date' | 'time'>('date');
  const [visibleStartTimePicker, setVisibleStartTimePicker] = React.useState(false);
  const [visibleDistancePicker, setVisibleDistancePicker] = React.useState(false);

  const toggleSportPicker = () => {
    setVisibleSportPicker(!visibleSportPicker);
  };

  const toggleDurationPicker = () => {
    setVisibleDurationPicker(!visibleDurationPicker);
  };
  
  const showStartTimePicker = (currentMode : 'date' | 'time') => {
    setVisibleStartTimePicker(true);
    setStartTimePickerMode(currentMode);
  };

  const toggleDistancePicker = () => {
    setVisibleDistancePicker(!visibleDistancePicker);
  };

  const getTotalTimeSeconds = () => {
    return parseInt(hours, 10) * 3600 + parseInt(minutes, 10) * 60 + parseInt(seconds, 10)
  };

  const getDistanceMeters = () => {
    let decimal = parseInt(distanceDecimal, 10);
    return parseInt(distance, 10) * 1000 + decimal * 100;
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
      
      <IconLabelContentPresenter onPress={() => showStartTimePicker('date')} iconName='calendar' iconType='antdesign' label='Dato' content={dateToString(startTime, 'date')} />

      <IconLabelContentPresenter onPress={() => showStartTimePicker('time')} iconName='clockcircleo' iconType='antdesign' label='Tidspunkt' content={dateToString(startTime, 'time')} />

      <IconLabelContentPresenter onPress={toggleDistancePicker} iconName='map-marker-distance' iconType='material-community' label='Lengde' content={metersToString(getDistanceMeters(), 2)} />

      <Overlay fullScreen isVisible={visibleSportPicker} onBackdropPress={toggleSportPicker}>
        <View style={styles.sportPicker}>
          <Text style={styles.cardHeader}>Velg aktivitet</Text>
          <FlatList
            data={Object.keys(Sport).filter((value) => isNaN(Number(value)) === true)}
            renderItem={({item, index}) => <IconLabelContentPresenter onPress={() => {setSport(item); toggleSportPicker();}} iconName='home' iconColor='#CC5C70' content={item} />}
            keyExtractor={(item) => item}>
          </FlatList>
        </View>
      </Overlay>

      <Overlay overlayStyle={{width: '80%'}} isVisible={visibleDurationPicker} onBackdropPress={toggleDurationPicker}>
        <View>
          <Text style={styles.cardHeader}>Varighet</Text>
          <View style={styles.durationPicker}>
            <View style={{flex: 1}}>            
              <Input
                keyboardType='numeric'
                style={styles.numberInput}
                onChangeText={value => setHours(value.replace(/\D/g,''))}
                value={hours}
              />
            </View>
            <Text style={styles.textBetweenNumberInput}>t</Text>
            <View style={{flex: 1}}>            
              <Input
                keyboardType='numeric'
                style={styles.numberInput}
                onChangeText={value => setMinutes(value.replace(/\D/g,''))}
                value={minutes}
              />
            </View>
            <Text style={styles.textBetweenNumberInput}>m</Text>
            <View style={{flex: 1}}>
              <Input
                keyboardType='numeric'
                style={styles.numberInput}
                onChangeText={value => setSeconds(value.replace(/\D/g,''))}
                value={seconds}
              />
            </View>
            <Text style={styles.textBetweenNumberInput}>s</Text>
          </View>
        </View>        
      </Overlay>

      {visibleStartTimePicker && (
        <DateTimePicker
          testID="dateTimePicker"
          value={startTime}
          mode={startTimePickerMode}
          is24Hour={true}
          display="default"
          onChange={onChange}
        />
      )}

      <Overlay overlayStyle={{width: '80%'}} isVisible={visibleDistancePicker} onBackdropPress={toggleDistancePicker}>
        <View>
          <Text style={styles.cardHeader}>Lengde</Text>
          <View style={styles.durationPicker}>
            <View style={{flex: 1}}>
              <Input
                style={styles.numberInput}
                selectTextOnFocus
                keyboardType='numeric'
                onChangeText={value => setDistance(value.replace(/\D/g,''))}
                value={distance}
              />
            </View>
            <Text style={styles.textBetweenNumberInput}>,</Text>
            <View style={{flex: 1}}>
              <Input
                style={styles.numberInput}
                selectTextOnFocus
                keyboardType='numeric'
                onChangeText={value => setDistanceDecimal(value.replace(/\D/g,''))}
                value={distanceDecimal}
              />
            </View>
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
  cardHeader: {
    fontWeight: 'bold',
    textTransform: 'uppercase',
    fontSize: 14,
  },
  sportPicker: {
    padding: 20,
  },
  durationPicker: {
    flexDirection: 'row',
    padding: 20,
  },
  numberInput: {
    textAlign: 'center',
    textAlignVertical: 'bottom',
  },  
  textBetweenNumberInput: {
    textAlignVertical: 'center'
  },
});
