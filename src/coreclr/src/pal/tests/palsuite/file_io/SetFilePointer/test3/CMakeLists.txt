set(SOURCES
  SetFilePointer.cpp
)

add_executable(paltest_setfilepointer_test3
  ${SOURCES}
)

add_dependencies(paltest_setfilepointer_test3 coreclrpal)

target_link_libraries(paltest_setfilepointer_test3
  ${COMMON_TEST_LIBRARIES}
)
