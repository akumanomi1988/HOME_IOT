Imports System
Imports System.Collections.Generic
Imports System.Net.Http
Imports System.Text
Imports System.Web.Http
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SMART_HOME_WS

<TestClass()> Public Class ValuesControllerTest
	<TestMethod()> Public Sub GetValues()
		'Disponer
		Dim controller As New ValuesController()

		'Actuar
		Dim result As IEnumerable(Of String) = controller.GetValues()

		'Declarar
		Assert.IsNotNull(result)
		Assert.AreEqual(2, result.Count())
		Assert.AreEqual("value1", result.ElementAt(0))
		Assert.AreEqual("value2", result.ElementAt(1))
	End Sub

	<TestMethod()> Public Sub GetValueById()
		'Disponer
		Dim controller As New ValuesController()

		'Actuar
		Dim result As String = controller.GetValue(5)

		'Declarar
		Assert.AreEqual("value", result)
	End Sub

	<TestMethod()> Public Sub PostValue()
		'Disponer
		Dim controller As New ValuesController()

		'Actuar
		controller.PostValue("value")

		'Declarar
	End Sub

	<TestMethod()> Public Sub PutValue()
		'Disponer
		Dim controller As New ValuesController()

		'Actuar
		controller.PutValue(5, "value")

		'Declarar
	End Sub

	<TestMethod()> Public Sub DeleteValue()
		'Disponer
		Dim controller As New ValuesController()

		'Actuar
		controller.DeleteValue(5)

		'Declarar
	End Sub
End Class
