import 'package:flutter/material.dart';
import 'home_page.dart';

void main() {
  runApp(HomePage());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'SMART HOME',
      theme: ThemeData(
        primarySwatch: Colors.lightGreen,
        visualDensity: VisualDensity.comfortable,
      ),
      home: HomePage(),
    );
  }
}
