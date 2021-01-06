import * as React from 'react';
import { GestureResponderEvent, StyleSheet, TextInput, TouchableOpacity } from 'react-native';

import { Text, View } from '../components/Themed';

interface LoginScreenProps {
    onPress: (event : GestureResponderEvent) => void;
}

export default function LoginScreen(props : LoginScreenProps) {
    return (
        <View style={styles.container}>
            <Text style={styles.logo}>Fit</Text>
            <TouchableOpacity style={styles.loginBtn} onPress={props.onPress}>
                <Text style={styles.loginText}>Logg inn med Google</Text>
            </TouchableOpacity>
        </View>
    );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#003f5c',
    alignItems: 'center',
    justifyContent: 'center',
  },
  logo:{
    fontWeight:"bold",
    fontSize:50,
    color:"#e69d17",
    marginBottom:40
  },
  loginBtn:{
    width:"80%",
    backgroundColor:"#e69d17",
    borderRadius:25,
    height:50,
    alignItems:"center",
    justifyContent:"center",
    marginTop:40,
    marginBottom:10,
  },
  loginText:{
    color:"white",
    textTransform: 'uppercase',
    fontWeight: 'bold',
  },
});