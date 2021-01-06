import * as React from 'react';
import { StyleSheet, StyleProp, ViewStyle } from 'react-native';

import { Text, View } from '../components/Themed';

interface CircleProps {
  radius: number;
  color?: string | undefined;
  style?: StyleProp<ViewStyle> | undefined;
  children?: 
    | JSX.Element
    | JSX.Element[]
    | string
    | string[];
}

const Circle = (props : CircleProps) => {
  return (    
    <View
      style={[props.style, {
        width: props.radius * 2,
        height: props.radius * 2,
        borderRadius: props.radius,
        backgroundColor: props.color,
      }]}>
        {props.children}
      </View>
  );
};

interface HalfCircleProps {
  radius: number;
  color?: string | undefined;
  rotation?: number | undefined;
  style?: StyleProp<ViewStyle> | undefined;
  children?: 
    | JSX.Element
    | JSX.Element[]
    | string
    | string[];
}

const HalfCircle = (props : HalfCircleProps) => {
  return (    
    <View
      style={[props.style, {
        flexDirection: 'row',
        transform: props.rotation ? [ { rotate: props.rotation.toString() + 'deg' } ] : undefined,
        backgroundColor: 'transparent',
      }]}>
      <View
        style={{
          width: props.radius,
          height: props.radius * 2,
          backgroundColor: props.color,
          borderRadius: props.radius,
          borderTopRightRadius: 0,
          borderBottomRightRadius: 0,
        }} />
        <View
          style={[{
            width: props.radius,
            height: props.radius * 2,
            backgroundColor: 'transparent',
          }]} />
    </View>
  );
};

interface QuarterCircleProps extends HalfCircleProps {
}

const QuarterCircle = (props : QuarterCircleProps) => {
  return (
    <View
      style={[props.style, {
        transform: props.rotation ? [ { rotate: props.rotation.toString() + 'deg' } ] : undefined,
        backgroundColor: 'transparent',
      }]}>
        <View
          style={{
            width: props.radius * 2,
            height: props.radius,
            backgroundColor: 'transparent',
          }} />
        <View
          style={{
            flexDirection: 'row',
            backgroundColor: 'transparent',
          }}>
            <View
              style={{
                width: props.radius,
                height: props.radius,
                backgroundColor: props.color,
                borderRadius: props.radius,
                borderTopRightRadius: 0,
                borderBottomRightRadius: 0,
                borderTopLeftRadius: 0,
              }} />
            <View
              style={[{
                width: props.radius,
                height: props.radius,
                backgroundColor: 'transparent',
              }]} />
        </View>
      </View>
  );
};

interface ProgressDiagramProps {
  percentage: number;
  expectedPercentage?: number | undefined;
  textTop?: string;
  textCenter?: string;
  textBottom?: string;
  color? : string | undefined;
  radius? : number | undefined;
}

export default function ProgressDiagram(props : ProgressDiagramProps) {

    const percentage = keepWithin(props.percentage, 0, 100);
    const expectedPercentage = props.expectedPercentage ? keepWithin(props.expectedPercentage, 0, 100) : 0;

    const radius = props.radius ? props.radius : 75;
    const shadowColor = '#c4c4c4';
    const expectedColor = '#9b9b9b';
    const color = props.color ? props.color : '#5cb0d6';
    const borderWidth = radius * 0.10;
    const innerColor = 'white';

    return (
      <View style={{ backgroundColor: 'transparent', padding: radius }}>
        <Circle style={styles.container} radius={radius} color={shadowColor} />

        {/* Percentage >= 50 */}
        {percentage >= 50 && <HalfCircle
          style={styles.container}
          radius={radius}
          color={color} />}
        {percentage >= 50 && expectedPercentage > percentage && <HalfCircle
          style={styles.container}
          radius={radius}
          color={expectedColor}
          rotation={3.6 * expectedPercentage - 180} />}
        {percentage >= 50 && <HalfCircle
          style={styles.container}
          radius={radius}
          color={color}
          rotation={3.6 * percentage - 180} />}

        {/* percentage < 50 && expectedPercentage < 50*/}
        {percentage < 50 && expectedPercentage < 50 && <HalfCircle
          style={styles.container}
          radius={radius}
          color={color} />}
        {percentage < 50 && expectedPercentage < 50 && expectedPercentage > percentage && <HalfCircle
          style={styles.container}
          radius={radius}
          color={expectedColor}
          rotation={3.6 * percentage} />}
        {percentage < 50 && expectedPercentage < 50 && <HalfCircle
          style={styles.container}
          radius={radius}
          color={shadowColor}
          rotation={expectedPercentage > percentage ? 3.6 * expectedPercentage : 3.6 * percentage} />}
        
        {/* percentage < 50 && expectedPercentage >= 50* && expectedPercentage - percentage >= 50 */}
        {percentage < 50 && expectedPercentage >= 50 && expectedPercentage - percentage >= 50 && <Circle style={styles.container} radius={radius} color={expectedColor} />}
        {percentage < 50 && expectedPercentage >= 50 && expectedPercentage - percentage >= 50 && <HalfCircle
          style={styles.container}
          radius={radius}
          color={shadowColor}
          rotation={3.6 * expectedPercentage} />}
        {percentage < 50 && expectedPercentage >= 50 && expectedPercentage - percentage >= 50 && <HalfCircle
          style={styles.container}
          radius={radius}
          color={color}
          rotation={0} />}
        {percentage < 50 && expectedPercentage >= 50 && expectedPercentage > percentage && expectedPercentage - percentage >= 50 && <HalfCircle
          style={styles.container}
          radius={radius}
          color={expectedColor}
          rotation={3.6 * percentage} />}
        
        {/* percentage < 50 && expectedPercentage >= 50 && expectedPercentage - percentage < 50 */}
        {percentage < 50 && expectedPercentage >= 50 && expectedPercentage - percentage < 50 && <HalfCircle
          style={styles.container}
          radius={radius}
          color={color} />}
        {percentage < 50 && expectedPercentage >= 50 && expectedPercentage - percentage < 50 && <QuarterCircle
          style={styles.container}
          radius={radius}
          color={expectedColor}
          rotation={3.6 * percentage} />}
        {percentage < 50 && expectedPercentage >= 50 && expectedPercentage - percentage < 50 && <QuarterCircle
          style={styles.container}
          radius={radius}
          color={expectedPercentage - percentage < 25 ? shadowColor : expectedColor}
          rotation={expectedPercentage - percentage < 25 ? 3.6 * expectedPercentage : 3.6 * expectedPercentage - 90} />}
        {<Circle 
          style={[styles.container, {
              margin: borderWidth,
              alignContent: 'center',
              justifyContent: 'center',
          }]}
          radius={radius - borderWidth}
          color={innerColor}>
            <Text style={styles.textTop}>{props.textTop}</Text>
            <Text style={styles.textCenter}>{props.textCenter}</Text>
            <Text style={styles.textBottom}>{props.textBottom}</Text>
          </Circle>}
      </View>
    );
}

const styles = StyleSheet.create({
  container: {
    position: 'absolute',
  },
  textTop: {
    flex: 0.5,
    color: 'black',
    textAlign: 'center',
    textAlignVertical: 'bottom',
  },
  textBottom: {
    flex: 0.5,
    color: 'black',
    textAlign: 'center',
    textAlignVertical: 'top',
  },
  textCenter: {
    fontSize: 40,
    color: 'black',
    fontWeight: 'bold',
    textAlign: 'center',
    textAlignVertical: 'center',
  }
});

function keepWithin(value : number, min : number, max : number) {
  return value > max ? max : (value < min ? min : value);
}