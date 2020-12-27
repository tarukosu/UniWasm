_```
cargo new <project_name> --lib
```

Add this to `Cargo.toml`

```toml
[dependencies]
uni-wasm = { path = "../../lib/uni-wasm" }

[lib]
crate-type = ['cdylib']
```

## Build

### Debug build
```
cargo wasi build
```

### Release build
```
cargo wasi build --release
```
