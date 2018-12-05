Imports System
Imports System.Collections
Imports System.Linq
Imports Telerik.Web.UI
Imports System.Data
Namespace FilterCustomEditors
    Public Class RadFilterDropDownEditor
        Inherits RadFilterDataFieldEditor
        Protected Overrides Sub CopySettings(ByVal baseEditor As RadFilterDataFieldEditor)
            MyBase.CopySettings(baseEditor)
            Dim editor = TryCast(baseEditor, RadFilterDropDownEditor)
            If editor IsNot Nothing Then
                DataSource = editor.DataSource
                DataTextField = editor.DataTextField
                DataValueField = editor.DataValueField
            End If
        End Sub

        Public Overrides Function ExtractValues() As System.Collections.ArrayList
            Dim list As New ArrayList()
            list.Add(_combo.SelectedItem.Value)
            Return list
        End Function

        Public Overrides Sub InitializeEditor(ByVal container As System.Web.UI.Control)
            _combo = New RadComboBox()
            _combo.ID = "MyCombo"
            _combo.DataTextField = DataTextField
            _combo.DataValueField = DataValueField
            _combo.DataSource = DataSource
            _combo.DataBind()
            _combo.Width = 200
            _combo.Height = 200
            _combo.Filter = RadComboBoxFilter.StartsWith
            _combo.Skin = "Simple"
            '_combo.CheckBoxes = True
            '_combo.EnableCheckAllItemsCheckBox = True

            container.Controls.Add(_combo)
        End Sub

        Public Overrides Sub SetEditorValues(ByVal values As System.Collections.ArrayList)
            If values IsNot Nothing AndAlso values.Count > 0 Then
                If values(0) Is Nothing Then
                    Return
                End If
                Dim item = _combo.FindItemByValue(values(0).ToString())
                If item IsNot Nothing Then
                    item.Selected = True
                    _combo.Enabled = True
                End If
            End If
        End Sub


        Public Property DataTextField() As String
            Get
                Return If(DirectCast(ViewState("DataTextField"), String), String.Empty)
            End Get
            Set(ByVal value As String)
                ViewState("DataTextField") = value
            End Set
        End Property
        Public Property DataValueField() As String
            Get
                Return If(DirectCast(ViewState("DataValueField"), String), String.Empty)
            End Get
            Set(ByVal value As String)
                ViewState("DataValueField") = value
            End Set
        End Property
        Public Property DataSource() As DataTable
            Get
                Return If(DirectCast(ViewState("DataSource"), DataTable), New DataTable())
            End Get
            Set(ByVal value As DataTable)
                ViewState("DataSource") = value
            End Set
        End Property

        Private _combo As RadComboBox
    End Class
End Namespace
