Imports System.Web.Mvc
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SMART_HOME_WS

<TestClass()> Public Class HomeControllerTest
	<TestMethod()> Public Sub Index()
		'Disponer
		Dim controller As New HomeController()

		'Actuar
		Dim result As ViewResult = DirectCast(controller.Index(), ViewResult)

		'Declarar
		Assert.IsNotNull(result)
		Dim viewData As ViewDataDictionary = result.ViewData
		Assert.AreEqual("Home Page", viewData("Title"))
	End Sub
End Class
