echo on
protoc --descriptor_set_out=Service.protobin --include_imports Service.proto




protogen Service.protobin


pause