import Data.Maybe (fromMaybe)
import Data.Version (showVersion)
import Paths_example (version)
import System.Environment (lookupEnv)
import Test.HUnit

main =
  runTestTT . TestCase $ do
    v <- lookupEnv "VERSION"
    fromMaybe "0.0.0" v @=? showVersion version
