import * as React from 'react';
import { StyleSheet } from 'react-native';

import { Text, View } from './Themed';

interface StatisticsDiagramProps {
  percentage: number;
  expectedPercentage?: number | undefined;
  textTop?: string;
  textCenter?: string;
  textBottom?: string;
  color? : string | undefined;
  radius? : number | undefined;
}

export default function StatisticsDiagram(props : StatisticsDiagramProps) {

    const color = props.color ? props.color : '#5cb0d6';
    const data = [
      {name: 'J', value: 13},
      {name: 'F', value: 8},
      {name: 'M', value: 12},
      {name: 'A', value: 8},
      {name: 'M', value: 12},
      {name: 'J', value: 6},
      {name: 'J', value: 7},
      {name: 'A', value: 11},
      {name: 'S', value: 10},
      {name: 'O', value: 11},
      {name: 'N', value: 6},
      {name: 'D', value: 16},
    ]

    const max = Math.max.apply(Math, data.map(function(o) { return o.value; }));  

    return (
      <View style={styles.container}>
        {data.map((value, index) => 
          <View style={styles.item} key={index}>
            <Text style={styles.caption}>{value.value}</Text>
            <View style={{backgroundColor: color, width: 10, borderRadius: 5, height: (100.0 * value.value)/max}}></View>
            <Text style={styles.caption}>{value.name}</Text>
          </View>)}        
      </View>
    );
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: 'transparent',
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-end',
  },
  item: {
    alignItems: 'center',
    margin: 5
  },
  caption: {
    color: 'grey',
  },
});