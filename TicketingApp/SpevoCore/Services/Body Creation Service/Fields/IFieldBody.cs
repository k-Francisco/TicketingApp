using SpevoCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpevoCore.Services.Body_Creation_Service.Fields
{
    public interface IFieldBody
    {
        //FieldModel CreateIntegerField(string title, bool required, bool enforceUniqueValues);
        TextFieldModel CreateTextField(string title, bool required, int maxLength);
        MultiLineTextFieldModel CreateNoteField(string title, bool required, int numberOfLines);
        DateTimeFieldModel CreateDateTimeField(string title, bool required, int displayFormat);
        ChoiceFieldModel CreateChoiceField(string title, List<string> choices, bool allowFillInChoices);
        LookupFieldModel CreateLookupField(string title, string lookupListId, string lookupFieldName);
        BooleanFieldModel CreateBooleanField(string title);
        NumberFieldModel CreateNumberField(string title, bool required, int minimumValue);
        CalculatedFieldModel CreateCalculatedField(string title, string formula, FieldTypeKind outputType);
        UserFieldModel CreateUserField(string title, int selectionGroup, int selectionMode);


    }
}
