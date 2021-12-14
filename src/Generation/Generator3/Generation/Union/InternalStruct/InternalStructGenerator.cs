﻿using Generator3.Converter;

namespace Generator3.Generation.Union
{
    public class InternalStructGenerator
    {
        private readonly Template<InternalStructModel> _template;
        private readonly Publisher _publisher;

        public InternalStructGenerator(Template<InternalStructModel> template, Publisher publisher)
        {
            _template = template;
            _publisher = publisher;
        }

        public void Generate(GirModel.Union union)
        {
            try
            {
                var model = new InternalStructModel(union);
                var source = _template.Render(model);
                var codeUnit = new CodeUnit(union.Namespace.GetCanonicalName(), $"{union.Name}.Struct", source);
                _publisher.Publish(codeUnit);
            }
            catch
            {
                Log.Warning($"Could not generate internal struct for union \"{union.Name}\"");
                throw;
            }
        }
    }
}
