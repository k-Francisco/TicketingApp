using System;
using System.Collections.Generic;
using System.Text;
using SpevoCore.Models;

namespace SpevoCore.Services.Body_Creation_Service.Fields
{
    public class FieldImplementation : IFieldBody
    {
        private const string FIELD_TYPE = "SP.Field";
        private const string CALCULATED_FIELD_TYPE = "SP.FieldCalculated";
        private const string DATETIME_FIELD_TYPE = "SP.FieldDateTime";
        private const string NUMBER_FIELD_TYPE = "SP.FieldNumber";
        private const string MULTILINETEXT_FIELD_TYPE = "SP.FieldMultiLineText";
        private const string LOOKUP_FIELD_TYPE = "SP.FieldCreationInformation";
        private const string USER_FIELD_TYPE = "SP.FieldUser";
        private const string CHOICE_FIELD_TYPE = "SP.FieldMultiChoice";

        public BooleanFieldModel CreateBooleanField(string title)
        {
            return new BooleanFieldModel() {
                Metadata = new Metadata() { Type = FIELD_TYPE },
                Title = title,
                FieldTypeKind = Convert.ToInt32(FieldTypeKind.Boolean),
            };
        }

        public CalculatedFieldModel CreateCalculatedField(string title, string formula, FieldTypeKind outputType)
        {
            return new CalculatedFieldModel()
            {
                Metadata = new Metadata() { Type = CALCULATED_FIELD_TYPE },
                FieldTypeKind = Convert.ToInt32(FieldTypeKind.Calculated),
                Title = title,
                Formula = formula,
                OutputType = Convert.ToInt32(outputType)
            };
        }

        public ChoiceFieldModel CreateChoiceField(string title, List<string> choices, bool allowFillInChoices)
        {
            return new ChoiceFieldModel()
            {
                Metadata = new Metadata() { Type = CHOICE_FIELD_TYPE },
                FieldTypeKind = Convert.ToInt32(FieldTypeKind.MultiChoice),
                Title = title,
                Choices = new Choices() { Metadata = new Metadata() { Type = "Collection(Edm.String)" }, Results = choices },
                FillInChoice = allowFillInChoices,
                DefaultValue = choices[0]
            };
        }

        public DateTimeFieldModel CreateDateTimeField(string title, bool required, int displayFormat)
        {
            return new DateTimeFieldModel()
            {
                Metadata = new Metadata() { Type = DATETIME_FIELD_TYPE },
                FieldTypeKind = Convert.ToInt32(FieldTypeKind.DateTime),
                Title = title,
                DisplayFormat = displayFormat,
                Required = required
            };
        }

        public LookupFieldModel CreateLookupField(string title, string lookupListId, string lookupFieldName)
        {
            return new LookupFieldModel()
            {
                LookupParameters = new LookupFieldModel.Parameters() {
                    Metadata = new Metadata() { Type = LOOKUP_FIELD_TYPE },
                    FieldTypeKind = Convert.ToInt32(FieldTypeKind.Lookup),
                    Title = title,
                    LookupFieldName = lookupFieldName,
                    LookupListId = lookupListId
                }
            };
        }

        public MultiLineTextFieldModel CreateNoteField(string title, bool required, int numberOfLines)
        {
            return new MultiLineTextFieldModel()
            {
                Metadata = new Metadata() { Type = NUMBER_FIELD_TYPE },
                FieldTypeKind = Convert.ToInt32(FieldTypeKind.Note),
                Title = title,
                NumberOfLines = numberOfLines,
                Required = required
            };
        }

        public NumberFieldModel CreateNumberField(string title, bool required, int minimumValue)
        {
            return new NumberFieldModel()
            {
                Metadata = new Metadata() { Type = NUMBER_FIELD_TYPE },
                FieldTypeKind = Convert.ToInt32(FieldTypeKind.Number),
                Title = title,
                MinimumValue = minimumValue,
                Required = required
            };
        }

        public TextFieldModel CreateTextField(string title, bool required, int maxLength)
        {
            return new TextFieldModel()
            {
                Metadata = new Metadata() { Type = NUMBER_FIELD_TYPE },
                FieldTypeKind = Convert.ToInt32(FieldTypeKind.Text),
                Title = title,
                MaxLength = maxLength.ToString(),
                Required = required
            };
        }

        public UserFieldModel CreateUserField(string title, int selectionGroup, int selectionMode)
        {
            return new UserFieldModel()
            {
                Metadata = new Metadata() { Type = USER_FIELD_TYPE },
                FieldTypeKind = Convert.ToInt32(FieldTypeKind.User),
                Title = title,
                SelectionGroup = selectionGroup,
                SelectionMode = selectionMode
            };
        }
    }
}
