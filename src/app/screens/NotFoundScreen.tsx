import { StackScreenProps } from '@react-navigation/stack';
import * as React from 'react';
import { StyleSheet, Text, TouchableOpacity, View } from 'react-native';

import { RootStackParamList } from '../types';

export default function NotFoundScreen({
  navigation,
}: StackScreenProps<RootStackParamList, 'NotFound'>) {
  return (
    <View style={styles.container}>
      <Text style={styles.logo}>Fit</Text>
      <Text style={styles.message}>Denne siden eksisterer ikke.</Text>
      <TouchableOpacity onPress={() => navigation.replace('Root')} style={styles.linkBtn}>
        <Text style={styles.linkText}>Gå til forsiden</Text>
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
    padding: 20,
  },
  logo:{
    fontWeight:"bold",
    fontSize:50,
    color:"#fb5b5a",
    marginBottom:40
  },
  message: {
    color:"white",
    fontSize: 20,
    fontWeight: 'bold',
  },
  linkBtn: {
    width:"80%",
    backgroundColor:"#fb5b5a",
    borderRadius:25,
    height:50,
    alignItems:"center",
    justifyContent:"center",
    marginTop:40,
    marginBottom:10,
  },
  linkText: {
    fontSize: 14,
    color:"white",
    textTransform: 'uppercase',
    fontWeight: 'bold',
  },
});
