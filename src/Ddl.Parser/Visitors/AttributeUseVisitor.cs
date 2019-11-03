﻿using TheToolsmiths.Ddl.Parser.Models;
using TheToolsmiths.Ddl.Parser.Utils;

namespace TheToolsmiths.Ddl.Parser.Visitors
{
    public class AttributeUseVisitor : BaseVisitor<IAttributeUse>
    {
        public override IAttributeUse VisitAttrUse(DdlParser.AttrUseContext context)
        {
            {
                var keyedAttrUse = context.keyedAttrUse();

                if (keyedAttrUse != null)
                {
                    var visitor = new KeyedAttributeUseVisitor();

                    return visitor.VisitKeyedAttrUse(keyedAttrUse);
                }
            }

            {
                var typedAttrUse = context.typedAttrUse();

                if (typedAttrUse != null)
                {
                    var visitor = new TypedAttributeUseVisitor();

                    return visitor.VisitTypedAttrUse(typedAttrUse);
                }
            }

            throw new System.NotImplementedException();
        }
    }

    public class KeyedAttributeUseVisitor : BaseVisitor<IKeyedAttributeUse>
    {
        public override IKeyedAttributeUse VisitKeyedAttrUse(DdlParser.KeyedAttrUseContext context)
        {
            Identifier key;
            {
                var identNode = context.Ident();

                key = IdentifierUtils.CreateIdentifier(identNode);
            }

            {
                var literalNode = context.Literal();

                if (literalNode != null)
                {
                    var literal = LiteralUtils.CreateLiteralInitialization(literalNode);

                    return new KeyedLiteralAttributeUse(key, literal);
                }
            }

            {
                var typedAttrUse = context.typedAttrUse();

                TypeIdentifier type;
                {
                    var typeIdentContext = typedAttrUse.typeIdent();

                    var visitor = new TypeIdentifierVisitor();

                    type = visitor.VisitTypeIdent(typeIdentContext);
                }

                StructValueInitialization initialization;
                {
                    var structInitContext = typedAttrUse.structValueInitialization();

                    var visitor = new StructValueInitializationVisitor();

                    initialization = visitor.VisitStructValueInitialization(structInitContext);
                }

                return new KeyedTypedAttributeUse(key, type, initialization);
            }
        }
    }

    public class TypedAttributeUseVisitor : BaseVisitor<ITypedAttributeUse>
    {
        public override ITypedAttributeUse VisitTypedAttrUse(DdlParser.TypedAttrUseContext context)
        {
            TypeIdentifier type;
            {
                var typeIdentContext = context.typeIdent();

                var visitor = new TypeIdentifierVisitor();

                type = visitor.VisitTypeIdent(typeIdentContext);
            }

            StructValueInitialization initialization;
            {
                var structInitContext = context.structValueInitialization();

                var visitor = new StructValueInitializationVisitor();

                initialization = visitor.VisitStructValueInitialization(structInitContext);
            }

            return new TypedAttributeUse(type, initialization);
        }
    }
}
