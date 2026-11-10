#!/bin/bash
set -u

UNITY="${UNITY_PATH:-/Applications/Unity/Hub/Editor/6000.3.20f1/Unity.app/Contents/MacOS/Unity}"
PROJECT="$(cd "$(dirname "$0")" && pwd)"
PLATFORM="${1:-}"

case "$PLATFORM" in
  mac)
    TARGET="OSXUniversal"
    METHOD="BuildScript.BuildMac"
    OUTPUT="$PROJECT/Builds/Mac/Game.app"
    ;;
  android)
    TARGET="Android"
    METHOD="BuildScript.BuildAndroid"
    OUTPUT="$PROJECT/Builds/Android/Game.apk"
    ;;
  *)
    echo "Usage: ./build.sh mac|android"
    exit 2
    ;;
esac

mkdir -p "$PROJECT/Logs"
LOG="$PROJECT/Logs/build_${PLATFORM}.log"

echo "Unity   : $UNITY"
echo "Project : $PROJECT"
echo "Target  : $TARGET"
echo "Output  : $OUTPUT"

"$UNITY" -batchmode -nographics -quit \
  -projectPath "$PROJECT" \
  -buildTarget "$TARGET" \
  -executeMethod "$METHOD" \
  -outputPath "$OUTPUT" \
  -logFile "$LOG"
RESULT=$?

if [ $RESULT -eq 0 ]; then
  if [ "$PLATFORM" = "mac" ]; then
    rm -f "$PROJECT/Builds/Mac/Game.zip"
    ditto -c -k --keepParent "$OUTPUT" "$PROJECT/Builds/Mac/Game.zip"
    echo "ZIPPED: $PROJECT/Builds/Mac/Game.zip"
  fi
  echo "BUILD SUCCEEDED: $OUTPUT"
else
  echo "BUILD FAILED (exit code $RESULT). Last lines of $LOG:"
  tail -n 30 "$LOG"
fi

exit $RESULT
