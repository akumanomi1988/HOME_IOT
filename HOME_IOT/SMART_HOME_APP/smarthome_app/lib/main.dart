import 'package:flutter/material.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'SMART HOME',
      theme: ThemeData(
        primarySwatch: Colors.lightGreen,
        visualDensity: VisualDensity.comfortable,
      ),
      home: MyHomePage(title: 'SMART HOME\nDashboard'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, this.title}) : super(key: key);
  final String title;
  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          widget.title,
          textAlign: TextAlign.center,
        ),
        centerTitle: true,
        titleSpacing: 2.0,
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.start,
          children: <Widget>[
            _DashBoardType1(
              
            )
            )
          ],
        ),
      ),
    );
  }
}

class DashBoardType1 extends StatefulWidget {
  final double  _temperatura;
  final double _humedad;

  double get temperatura => this._temperatura;
  double get humedad => this._humedad;

  set temperatura(double value) => this._temperatura;
  set humedad(double value) => this._temperatura;

  DashBoardType1(this._temperatura, this._humedad);
_@override
  _DashBoardType1 createState() => _DashBoardType1();
}

class _DashBoardType1 extends State<DashBoardType1> {
  @override
  Widget build(BuildContext context) {
    return Row(
              children: <Widget>[
                Text('Temperatura: Cº',
                    textAlign: TextAlign.left,
                    style:
                        TextStyle(fontWeight: FontWeight.w900, fontSize: 20)),
                Text('Humedad:     %',
                    textAlign: TextAlign.right,
                    style:
                        TextStyle(fontWeight: FontWeight.bold, fontSize: 20)),
              ],
            );
  }
}