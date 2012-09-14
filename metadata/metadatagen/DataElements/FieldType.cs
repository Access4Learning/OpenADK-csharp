using System;

namespace Edustructures.Metadata.DataElements
{
    public class FieldType
    {
        private static FieldType FIELD_BOOLEAN = new FieldType( AdkDataType.Boolean );
        private static FieldType FIELD_STRING = new FieldType( AdkDataType.String );
        private static FieldType FIELD_DATE = new FieldType( AdkDataType.Date );
        private static FieldType FIELD_TIME = new FieldType( AdkDataType.Time );
        private static FieldType FIELD_DATETIME = new FieldType( AdkDataType.Datetime );
        private static FieldType FIELD_DURATION = new FieldType( AdkDataType.Duration );
        private static FieldType FIELD_INT = new FieldType( AdkDataType.Int );
        private static FieldType FIELD_UINT = new FieldType( AdkDataType.Uint );
        private static FieldType FIELD_DECIMAL = new FieldType( AdkDataType.Decimal );
        private static FieldType FIELD_SIFVERSION = new FieldType( AdkDataType.Sifversion );

        private static FieldType FIELD_XMLAny = new FieldType( AdkDataType.Any );

        private AdkDataType fDataType;
        private String fClassType;

        private FieldType( AdkDataType valueType )
        {
            fDataType = valueType;
        }

        public bool IsComplex
        {
            get { return fDataType == AdkDataType.Complex; }
        }

        public bool IsEnum
        {
            get { return fDataType == AdkDataType.Enum; }
        }

        public bool IsSimpleType
        {
            get { return !(IsComplex || IsEnum); }
        }

        public String ClassType
        {
            get { return fClassType; }
        }

        public String Enum
        {
            get
            {
                if ( fDataType == AdkDataType.Enum ) {
                    return fClassType;
                }
                return null;
            }
        }


        public AdkDataType DataType
        {
            get { return fDataType; }
        }

        public static FieldType GetFieldType( String classType )
        {
            if ( classType == null ||
                 classType.Length == 0 ||
                 String.Compare( classType, "String", true ) == 0 ||
                 String.Compare( classType, "Token", true ) == 0 ||
                 String.Compare( classType, "IdRefType", true ) == 0 ||
                 String.Compare( classType, "NormalizedString", true ) == 0 ||
                 String.Compare( classType, "NCName", true ) == 0 ||
                 String.Compare( classType, "AnyUri", true ) == 0 ||
                 String.Compare( classType, "Language", true ) == 0 ||
                 String.Compare( classType, "AnyAtomicType", true ) == 0 ) {
                return FIELD_STRING;
            }
            else if ( String.Compare( classType, "DateTime", true ) == 0 ) {
                return FIELD_DATETIME;
            }
            else if ( String.Compare( classType, "Int", true ) == 0 ||
                      String.Compare( classType, "PositiveInteger", true ) == 0 ||
                      String.Compare( classType, "NonNegativeInteger", true ) == 0 ||
                      String.Compare( classType, "GYear", true ) == 0 ||
                      String.Compare( classType, "GMonth", true ) == 0 ||
                      String.Compare( classType, "GDay", true ) == 0 ) {
                return FIELD_INT;
            }
            else if ( String.Compare( classType, "UnsignedInt", true ) == 0 ||
                      String.Compare(classType, "UINT", true) == 0)
            {
                return FIELD_UINT;
            }
            else if ( String.Compare( classType, "Decimal", true ) == 0 ) {
                return FIELD_DECIMAL;
            }
            else if ( String.Compare( classType, "SIFDate", true ) == 0 ||
                      String.Compare( classType, "Date", true ) == 0 ) {
                return FIELD_DATE;
            }
            else if ( String.Compare( classType, "Boolean", true ) == 0 ||
                      String.Compare( classType, "YesNo", true ) == 0 ) {
                return FIELD_BOOLEAN;
            }
            else if ( String.Compare( classType, "SIFSimpleTime", true ) == 0 ||
                      String.Compare( classType, "Time", true ) == 0 ||
                      String.Compare( classType, "SIFTime", true ) == 0 ) {
                return FIELD_TIME;
            }
            else if ( String.Compare( classType, "SIFVersion", true ) == 0 ) {
                return FIELD_SIFVERSION;
            }
            else if( String.Compare( classType, "Duration", true ) == 0 )
            {
                return FIELD_DURATION;
            }
            else {
                FieldType returnValue = new FieldType( AdkDataType.Complex );
                returnValue.fClassType = classType;
                return returnValue;
            }
        }


        public string MetadataType
        {
            get
            {
                switch ( fDataType ) {
                    case AdkDataType.Any:
                        return "Any";
                    case AdkDataType.Boolean:
                        return "Boolean";
                    case AdkDataType.Complex:
                        return fClassType;
                    case AdkDataType.Date:
                        return "Date";
                    case AdkDataType.Datetime:
                        return "DateTime";
                    case AdkDataType.Decimal:
                        return "Decimal";
                    case AdkDataType.Enum:
                        return fClassType;
                    case AdkDataType.Int:
                        return "int";
                    case AdkDataType.Sifversion:
                        return "SifVersion";
                    case AdkDataType.String:
                        return "String";
                    case AdkDataType.Time:
                        return "Time";
                    case AdkDataType.Uint:
                        return "Uint";
                }
                return null;
            }
        }


        public static FieldType ToEnumType( FieldType existingField,
                                            String enumName )
        {
            if ( existingField.DataType == AdkDataType.Enum ) {
                if ( existingField.ClassType.Equals( enumName ) ) {
                    return existingField;
                }
                else {
                    throw new ParseException
                        ( "Field was already defined as an Enum with a different name: " +
                          existingField.DataType + " { ENUM:" + enumName + "}" );
                }
            }
            else {
                if ( existingField.DataType != AdkDataType.String ) {
                    throw new ParseException
                        ( "Cannot define an enum for a type other than a String. Field:" +
                          existingField.DataType + " { ENUM:" + enumName + "}" );
                }
            }
            // TODO: We could support "YesNo" values as boolean fields, but we need to be able
            // to format them differently than booleans....
            //		if( enumName.equals( "YesNo" )){
            //			return FIELD_BOOLEAN;
            //		}


            FieldType returnValue = new FieldType( AdkDataType.Enum );
            returnValue.fClassType = enumName;
            return returnValue;
        }

        public override int GetHashCode()
        {
            if ( fDataType == AdkDataType.Complex || fDataType == AdkDataType.Enum ) {
                return fClassType.GetHashCode();
            }
            else {
                return fDataType.GetHashCode();
            }
        }


        public override bool Equals( Object o )
        {
            if ( this == o ) {
                return true;
            }
            if ( o != null ) {
                FieldType compared = o as FieldType;
                if ( compared != null ) {
                    if ( fDataType == AdkDataType.Complex || fDataType == AdkDataType.Enum ) {
                        return fClassType.Equals( compared.fClassType );
                    }
                    else {
                        return fDataType == compared.fDataType;
                    }
                }
            }
            return false;
        }

        public override String ToString()
        {
            if ( IsComplex ) {
                return "Complex Field: " + ClassType;
            }
            else if ( IsEnum ) {
                return "Enum Field: " + Enum;
            }
            else {
                return "Simple Field: " + fDataType;
            }
        }
    }
}