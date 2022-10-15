import 'package:flutter/material.dart';

class DeviceReport extends StatefulWidget {
  // final String _Dispositivos = ['Salon', 'Dormitorio 1', 'Dormitorio 2', 'Dormitorio 3'];
  final String deviceName;
  DeviceReport({Key key, this.deviceName});
  @override
  _DeviceReport createState() => _DeviceReport();
}

class _DeviceReport extends State<DeviceReport> {
  @override
  Widget build(BuildContext context) {
    return Container(
        padding: const EdgeInsets.symmetric(horizontal: 20.0, vertical: 10.0),
        margin: EdgeInsets.all(10.0),
        decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(12),
            border: Border.all(width: 0.4, color: Colors.lightBlue[300])),
        child: Column(children: <Widget>[
          Text(
            'Dormitorio',
            style: TextStyle(
              fontSize: 24.0,
            ),
          ),
          Row(
            children: <Widget>[
              Container(
                padding: const EdgeInsets.symmetric(
                    horizontal: 20.0, vertical: 10.0),
                decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(12),
                    border:
                        Border.all(width: 0.4, color: Colors.lightBlue[300])),
                child: Column(
                  children: <Widget>[
                    Text(
                      'MAX',
                    ),
                    Text('Min'),
                  ],
                ),
              ),
              Container(
                padding: const EdgeInsets.symmetric(
                    horizontal: 20.0, vertical: 10.0),
                decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(12),
                    border:
                        Border.all(width: 0.4, color: Colors.lightBlue[300])),
                child: Row(
                  children: <Widget>[
                    Text('Temperatura'),
                    Text('Humedad'),
                  ],
                ),
              ),
              Container(
                padding: const EdgeInsets.symmetric(
                    horizontal: 20.0, vertical: 10.0),
                decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(12),
                    border:
                        Border.all(width: 0.4, color: Colors.lightBlue[300])),
                child: Column(
                  children: <Widget>[
                    Text('MAX'),
                    Text('MIN'),
                  ],
                ),
              )
            ],
          )
        ]));
  }
}
