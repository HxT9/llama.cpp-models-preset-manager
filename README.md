# llama.cpp --models-preset manager

A Windows Forms application to manage [llama.cpp](https://github.com/ggml-org/llama.cpp) configuration file for the --models-preset option.
https://github.com/ggml-org/llama.cpp/blob/master/tools/server/README.md#using-multiple-models

## Features

- **Model Management**:
  - Add, edit, and remove AI models (can use multiple instances of the same model with different flags, just use different names).
  - **Auto-Scan**: Quickly add multiple GGUF models by scanning a directory.

- **Configuration / Flags**:
  - Assign specific command-line flags to each model (e.g., `c`, `ngl`, `mmproj`).
  - Dropdown selection for a list of already used flags.

- **Persistence**:
  - All data is saved automatically to a local SQLite database.
  - Configuration export to `.ini` format for usage with llama-server --models-preset

## Usage

1. **Adding Models**:
   - Use `Actions -> Lookup from folder` to scan for `.gguf` files.
   - Or manually add rows in the left grid.
2. **Editing Flags**:
   - Select a model on the left.
   - in the right grid, add flags without '-' (e.g., Flag: `c`, Value: `4096`; or flag with empty value like `no-mmap`).
   - Use the `•` button to pick from saved common flags.
3. **Managing Flags**:
   - `Actions -> Flag list` to open the global flag list (populated with used flags).
4. **Export**:
   - `Actions -> Export models-preset config` to generate the configuration file.

## Requirements
- .NET 8.0 (Windows)
