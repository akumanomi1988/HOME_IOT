Public Class SMART_HOME
	Public Class HEATING
		Private _HConfiguration As HCONFIGURATION
		Public Property HConfiguration() As HCONFIGURATION
			Get
				Return _HConfiguration
			End Get
			Set(ByVal value As HCONFIGURATION)
				_HConfiguration = value
			End Set
		End Property
		Private _DeviceList As List(Of DEVICE)
		Public Property DeviceList() As List(Of DEVICE)
			Get
				Return _DeviceList
			End Get
			Set(ByVal value As List(Of DEVICE))
				_DeviceList = value
			End Set
		End Property
	End Class
	Public Class HCONFIGURATION

	End Class
	Public Class DEVICE

	End Class
End Class
